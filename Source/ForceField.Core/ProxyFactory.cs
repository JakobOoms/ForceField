using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ForceField.Core.Generator;
using Roslyn.Scripting;
using Roslyn.Scripting.CSharp;

namespace ForceField.Core
{
    public static class ProxyFactory
    {
        private static readonly Dictionary<Type, ProxyInstantiator> Instantiators = new Dictionary<Type, ProxyInstantiator>();

        public static object Create(Type type, object implementation, AdvisorsConfiguration configuration)
        {
            var typeHasInterestedAdvices = configuration.AppliedAdvices.Any(appliedAdvice => appliedAdvice.IsApplicableFor(type));

            //If there are no advices that are applicable for this type, the type should not be decorated and we can easily
            //return the original implementation.
            if (!typeHasInterestedAdvices || configuration.IsBlockedType(type))
                return implementation;

            ProxyInstantiator proxyInstantiator;
            if (!Instantiators.TryGetValue(type, out proxyInstantiator))
            {
                proxyInstantiator = BuildInstantiator(type);
                Instantiators[type] = proxyInstantiator;
            }

            var strippedConfiguration = configuration.CreateCopyFor(type);
            return proxyInstantiator.Create(implementation, strippedConfiguration);
        }

        public static T Create<T>(T implementation, AdvisorsConfiguration configuration) where T : class
        {
            Guard.ArgumentNotNull(() => implementation);
            Guard.ArgumentNotNull(() => configuration);

            var type = typeof(T);
            return (T)Create(type, implementation, configuration);
        }

        private static IEnumerable<string> GetRequiredAssemblies(Type type)
        {
            var assemblyLocations = new List<string>();
            var currentAssembly = type.Assembly;
            var referencedAssemblies = currentAssembly.GetReferencedAssemblies();
            assemblyLocations.Add(currentAssembly.Location);
            assemblyLocations.AddRange(referencedAssemblies.Select(Assembly.Load).Select(assembly => assembly.Location));
            return assemblyLocations;
        }

        private static ProxyInstantiator BuildInstantiator(Type type)
        {
            var proxyGenerator = new ProxyGenerator();
            var generatedClassResult = proxyGenerator.Generate(type);

            var hostingContainer = new HostingContainer();
            var scriptEngine = new ScriptEngine(GetRequiredAssemblies(type));
            var scriptSession = Session.Create(hostingContainer);

            scriptEngine.Execute(generatedClassResult.Code, scriptSession);
            scriptEngine.Execute(@"ProxyInstantiator = new ProxyInstantiator((innerTarget,configuration) => new " + generatedClassResult.GeneratedClassName + "((" + type.FullName + ")innerTarget, configuration));", scriptSession);
            return hostingContainer.ProxyInstantiator;
        }
    }
}