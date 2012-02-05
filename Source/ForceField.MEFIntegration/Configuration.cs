using ForceField.Core;

namespace ForceField.MEFIntegration
{
    public class Configuration : BaseConfiguration
    {
        private ForceFieldContainer _container;

        protected override T ResolveAdvice<T>()
        {
            return _container.GetExportedValue<T>();
        }

        internal void SetInnerContainer(ForceFieldContainer container)
        {
            _container = container;
        }

        protected override BaseConfiguration Clone()
        {
            return new Configuration { _container = _container };
        }
    }
}
