using Autofac;
using ForceField.Core;

namespace ForceField.AutofacIntegration
{
    public class AutofacAdvisorConfiguration : AdvisorsConfiguration
    {
        private IContainer _container;

        internal void SetContainer(IContainer container)
        {
            _container = container;
        }

        protected override T TryResolveAdvice<T>()
        {
            return _container.ResolveOptional<T>();
        }

        protected override AdvisorsConfiguration Clone()
        {
            var clone = new AutofacAdvisorConfiguration();
            clone.SetContainer(_container);
            return clone;
        }
    }
}
