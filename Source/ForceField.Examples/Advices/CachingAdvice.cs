using System.Globalization;
using System.Text;
using ForceField.Core.Advices;
using ForceField.Core.Invocation;
using ForceField.Examples.CachingInfractructure;
using ForceField.Examples.Services;

namespace ForceField.Examples.Advices
{
    public class CachingAdvice : IAdvice
    {
        private readonly ICacheProvider _cacheProvider;
        private readonly CacheConfiguration _cacheConfiguration;

        public CachingAdvice(ICacheProvider cacheProvider, ICacheConfigurationService configurationService)
        {
            _cacheProvider = cacheProvider;
            _cacheConfiguration = configurationService.BuildConfiguration();
        }

        public void ApplyAdvice(IInvocation invocation)
        {
            if (_cacheConfiguration.ShouldCache(invocation.MethodInfo))
            {
                var cacheKey = GetCacheKeyForInvocation(invocation);
                object cachedValue;
                if (_cacheProvider.TryGet(cacheKey, out cachedValue))
                {
                    invocation.ReturnValue = cachedValue;
                }
                else
                {
                    invocation.Proceed();
                    _cacheProvider.Store(cacheKey, invocation.ReturnValue);
                }
            }
            else
            {
                invocation.Proceed();
            }
        }

        private string GetCacheKeyForInvocation(IInvocation invocation)
        {
            var builder = new StringBuilder();
            builder.Append(invocation.MethodInfo.DeclaringType.FullName).Append(".").Append(invocation.MethodInfo.Name);
            foreach (var arg in invocation.Arguments)
            {
                builder.Append(arg.Parameter.Name);
                builder.Append(arg.Value == null ? "null" : arg.Value.GetHashCode().ToString(CultureInfo.InvariantCulture));
            }
            return builder.ToString();
        }
    }
}
