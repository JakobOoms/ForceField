using System;

namespace ForceField.Core
{
    public class ProxyInstantiator
    {
        private readonly Func<object, BaseConfiguration, object> _creator;

        public ProxyInstantiator(Func<object, BaseConfiguration, object> creator)
        {
            _creator = creator;
        }

        public T Create<T>(T implementation, BaseConfiguration configuration)
        {
            return (T)_creator(implementation, configuration);
        }
    }
}