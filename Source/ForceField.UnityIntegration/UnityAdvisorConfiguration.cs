using System.Linq;
using ForceField.Core;
using Microsoft.Practices.Unity;

namespace ForceField.UnityIntegration
{
    public class UnityAdvisorConfiguration : AdvisorsConfiguration
    {
        private IUnityContainer _innerContainer;

        internal void SetInnerContainer(IUnityContainer container)
        {
            _innerContainer = container;
        }

        protected override TAdvice TryResolveAdvice<TAdvice>()
        {
            return _innerContainer.Registrations.Any(registration => registration.RegisteredType == typeof(TAdvice)) ? _innerContainer.Resolve<TAdvice>() : null;
        }

        protected override AdvisorsConfiguration Clone()
        {
            var clone = new UnityAdvisorConfiguration();
            clone.SetInnerContainer(_innerContainer);
            return clone;
        }
    }
}
