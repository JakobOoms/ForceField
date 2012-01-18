using System.Linq;
using Autofac;
using ForceField.Core;

namespace ForceField.AutofacIntegration
{
    public static class AutofacContainerExtensions
    {
        public static IAOPAutofacContainer Build(this ContainerBuilder builder, AutofacAdvisorConfiguration configuration)
        {
            //Register all advices into the container, so they can be resolved
            foreach (var adviceType in configuration.GetRegisteredAdvices())
                builder.RegisterType(adviceType);

            var innerContainer = builder.Build();
            //Inject the builded container so the configuration can resolve the builded advices when needed
            configuration.SetContainer(innerContainer);
            return new WrappedAutofacContainer(innerContainer, configuration);
        }

        public static T Resolve<T>(this IAOPAutofacContainer container) where T : class
        {
            //cast back to make sure we're not in an infinite recursion.
            var innerService = ((IContainer)container).Resolve<T>();
            return ProxyFactory.Create(innerService, container.Configuration);
        }
    }
}