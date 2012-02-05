using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ForceField.Core.Extensions;
using ForceField.Core.Generator;
using Roslyn.Scripting;
using Roslyn.Scripting.CSharp;

namespace ForceField.Core
{
    public static class ProxyFactory
    {
        private static readonly Dictionary<Type, ProxyInstantiator> Instantiators = new Dictionary<Type, ProxyInstantiator>();

        public static object Create(Type type, object implementation, BaseConfiguration configuration)
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

        public static T Create<T>(T implementation, BaseConfiguration configuration) where T : class
        {
            Guard.ArgumentIsNotNull(() => implementation);
            Guard.ArgumentIsNotNull(() => configuration);

            var type = typeof(T);
            return (T)Create(type, implementation, configuration);
        }

        private static IEnumerable<string> GetRequiredAssemblies(Type type)
        {
            var assemblyLocations = new HashSet<string>();
            var currentAssembly = type.Assembly;
            assemblyLocations.Add(currentAssembly.Location);
            var hoster = new AssemblyNameToAssemblyLocationMapper();
            var resolved = hoster.GetAssemblyLocations(currentAssembly.GetReferencedAssemblies());
            assemblyLocations.AddRange(resolved);
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
            scriptEngine.Execute(@"ProxyInstantiator = new ProxyInstantiator((innerTarget,configuration) => new " + generatedClassResult.GeneratedClassName + "((" + type.GetFullName() + ")innerTarget, configuration));", scriptSession);
            return hostingContainer.ProxyInstantiator;
        }
    }

    internal class AssemblyNameToAssemblyLocationMapper
    {
        public IEnumerable<string> GetAssemblyLocations(ICollection<AssemblyName> assemblyNames)
        {
            //If no assemblynames are passed, don't waste any time on creating, loading and unloading a temp appdomain.
            if (assemblyNames.Count == 0)
                return Enumerable.Empty<string>();

            var hashSet = new HashSet<string>();
            var tempAppDomain = AppDomain.CreateDomain("ForceField_TempAppDomain_" + Guid.NewGuid(), null, AppDomain.CurrentDomain.SetupInformation);
            var runner = new LocationExtractor(assemblyNames.Select(x => x.FullName).ToList());
            tempAppDomain.DoCallBack(runner.SetLocations);
            hashSet.AddRange((IEnumerable<string>)tempAppDomain.GetData(LocationExtractor.LocationsKey));
            hashSet.Add(typeof(Enumerable).Assembly.Location);
            AppDomain.Unload(tempAppDomain);
            return hashSet;
        }

        [Serializable]
        private class LocationExtractor
        {
            public const string LocationsKey = "locations";
            private readonly IEnumerable<string> _assemblyFullNames;

            public LocationExtractor(IEnumerable<string> assemblyFullNames)
            {
                _assemblyFullNames = assemblyFullNames;
            }

            public void SetLocations()
            {
                var domain = AppDomain.CurrentDomain;
                var locations = _assemblyFullNames.Select(domain.Load).Select(ass => ass.Location).ToList();
                domain.SetData(LocationsKey, locations);
            }
        }
    }
}