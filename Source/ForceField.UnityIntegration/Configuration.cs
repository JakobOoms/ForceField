using System.Linq;
using ForceField.Core;
using Microsoft.Practices.Unity;

namespace ForceField.UnityIntegration
{
    public class Configuration : BaseConfiguration
    {
        private IUnityContainer _innerContainer;

        internal void SetInnerContainer(IUnityContainer container)
        {
            _innerContainer = container;
        }

        protected override TAdvice ResolveAdvice<TAdvice>()
        {
            return _innerContainer.Registrations.Any(registration => registration.RegisteredType == typeof(TAdvice)) ? _innerContainer.Resolve<TAdvice>() : null;
        }

        protected override BaseConfiguration Clone()
        {
            var clone = new Configuration();
            clone.SetInnerContainer(_innerContainer);
            return clone;
        }
    }
}
