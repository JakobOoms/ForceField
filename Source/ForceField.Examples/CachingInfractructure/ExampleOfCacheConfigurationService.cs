using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ForceField.Examples.CachingInfractructure
{
    public class ExampleOfCacheConfigurationService : ICacheConfigurationService
    {
        public CacheConfiguration BuildConfiguration()
        {
            var fixedForOneHour = new CacheInstruction(CacheType.Fixed, TimeSpan.FromHours(1));
            var cacheConfig = new CacheConfiguration();
            cacheConfig.ApplyCachingOn(x => x.DeclaringType.Name.EndsWith("Repository"), fixedForOneHour)
                .InvalidateOn(x => x.Name.StartsWith("Save"))
                .InvalidateOn(x => x.Name.StartsWith("Delete"))
                .InvalidateOn(x => x.Name.StartsWith("Update"));

            //apply exceptions
            //cacheConfig
            //    .ExceptFor<IPersonRepository>(x => x.GetByName(null))
            //    .InvalidateOn<IPersonRepository>(x => x.Save(null));
            return cacheConfig;
        }
    }
}
