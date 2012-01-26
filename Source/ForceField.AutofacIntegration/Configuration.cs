using Autofac;
using ForceField.Core;

namespace ForceField.AutofacIntegration
{
    public class Configuration : BaseConfiguration
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

        protected override BaseConfiguration Clone()
        {
            var clone = new Configuration();
            clone.SetContainer(_container);
            return clone;
        }
    }
}
