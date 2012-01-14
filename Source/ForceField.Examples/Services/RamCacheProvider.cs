using System.Collections.Generic;

namespace ForceField.Examples.Services
{
    public class RamCacheProvider : ICacheProvider
    {
        private readonly Dictionary<string, object> _cachedValues = new Dictionary<string, object>();
        public void Store(string key, object value)
        {
            _cachedValues[key] = value;
        }

        public void Invalidate(string key)
        {
            _cachedValues.Remove(key);
        }

        public bool TryGet(string key, out object valueInCache)
        {
            return _cachedValues.TryGetValue(key, out valueInCache);
        }
    }
}