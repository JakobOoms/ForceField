using System;

namespace ForceField.Core
{
    public class ProxyInstantiator
    {
        private readonly Func<object, AdvisorsConfiguration, object> _creator;

        public ProxyInstantiator(Func<object, AdvisorsConfiguration, object> creator)
        {
            _creator = creator;
        }

        public T Create<T>(T implementation, AdvisorsConfiguration configuration)
        {
            return (T)_creator(implementation, configuration);
        }
    }
}