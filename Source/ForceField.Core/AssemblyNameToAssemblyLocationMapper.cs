using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ForceField.Core
{
    internal class AssemblyNameToAssemblyLocationMapper
    {
        public IEnumerable<string> GetAssemblyLocations(ICollection<AssemblyName> assemblyNames)
        {
            //If no assemblynames are passed, don't waste any time on creating, loading and unloading a temp appdomain.
            if (assemblyNames.Count == 0)
                return Enumerable.Empty<string>();

            var tempAppDomain = AppDomain.CreateDomain("ForceField_TempAppDomain_" + Guid.NewGuid(), null, new AppDomainSetup());
            var runner = new LocationExtractor(assemblyNames.Select(x => x.FullName).ToList());
            tempAppDomain.DoCallBack(runner.SetLocations);
            var result = (List<string>)tempAppDomain.GetData(LocationExtractor.ResultKey);
            AppDomain.Unload(tempAppDomain);
            return result;
        }

        [Serializable]
        private class LocationExtractor
        {
            public const string ResultKey = "locations";
            private readonly IEnumerable<string> _assemblyFullNames;

            public LocationExtractor(IEnumerable<string> assemblyFullNames)
            {
                _assemblyFullNames = assemblyFullNames;
            }

            public void SetLocations()
            {
                var domain = AppDomain.CurrentDomain;
                var locations = _assemblyFullNames.Select(domain.Load).Select(ass => ass.Location).ToList();
                domain.SetData(ResultKey, locations);
            }
        }
    }
}