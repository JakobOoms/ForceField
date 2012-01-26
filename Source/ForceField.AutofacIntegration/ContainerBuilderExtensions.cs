using System.Linq;
using Autofac;
using ForceField.Core;

namespace ForceField.AutofacIntegration
{
    /// <summary>
    /// Acts as an 'entry point' for the end user by using an 'overload' of the build method, in which they can pass the configuration
    /// that should be applied to the registered services.
    /// </summary>
    public static class ContainerBuilderExtensions
    {
        /// <summary>
        /// Builds the autofac container and applies the advices from the configuration to the registered services
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IForceFieldAutofacContainer Build(this ContainerBuilder builder, Configuration configuration)
        {
            Guard.ArgumentIsNotNull(() => builder);
            Guard.ArgumentIsNotNull(() => configuration);

            //Register all advices into the container, so they can be resolved
            foreach (var adviceType in configuration.GetRegisteredAdvices())
                builder.RegisterType(adviceType);

            //Build the real autofac container and set it as inner container
            var innerContainer = builder.Build();
            //Inject the builded autofac container into the configuration so it can resolve the builded advices when needed
            configuration.SetContainer(innerContainer);
            return new ForceFieldAutofacContainer(innerContainer, configuration);
        }
    }
}