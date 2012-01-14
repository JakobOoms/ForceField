using Autofac;
using ForceField.Core;

namespace ForceField.AutofacIntegration
{
    public static class AutofacContainerExtensions
    {
        public static IAOPAutofacContainer Build(this ContainerBuilder builder, AutofacAdvisorConfiguration configuration)
        {
            var innerContainer = builder.Build();
            configuration.SetContainer(innerContainer);
            return new WrappedAutofacContainer(innerContainer, configuration);
        }

        public static T Resolve<T>(this IAOPAutofacContainer container)
        {
            //cast back to make sure we're not in an infinite recursion.
            var innerService = ((IContainer)container).Resolve<T>();
            return ProxyFactory.Create(innerService, container.Configuration);
        }
    }
}