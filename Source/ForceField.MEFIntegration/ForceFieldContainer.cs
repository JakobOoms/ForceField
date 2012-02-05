using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Text;
using ForceField.Core;

namespace ForceField.MEFIntegration
{
    public class ForceFieldContainer : CompositionContainer
    {
        private readonly Configuration _config;

        public ForceFieldContainer(ComposablePartCatalog catalog, Configuration config)
            : base(catalog)
        {
            Guard.ArgumentIsNotNull(() => catalog, () => config);

            _config = config;
            _config.SetInnerContainer(this);
        }

        protected override IEnumerable<Export> GetExportsCore(ImportDefinition definition, AtomicComposition atomicComposition)
        {
            var baseExports = base.GetExportsCore(definition, atomicComposition).ToList();
            Type typeToExport = null;

            //First, find the actual type that is being imported: There seems to be no way to retrieve the actual type that was used 
            //during the export, so the only way we can find the type is to re-apply the 'logic' to create the contract name, based on a given type.
            foreach (var export in baseExports)
            {
                var valueType = export.Value.GetType();
                typeToExport = GetMEFContractName(valueType) == definition.ContractName ? valueType : valueType.GetInterfaces().First(x => GetMEFContractName(x) == definition.ContractName);
            }
            //Convert all the export to a ForceFieldExport, which will create a Proxy when needed
            return baseExports.Select(export => new ForceFieldExport(_config, export, typeToExport)).ToList();
        }

        private string GetMEFContractName(Type type)
        {
            var name = new StringBuilder();
            if (!string.IsNullOrEmpty(type.Namespace))
            {
                name.Append(type.Namespace);
                name.Append(".");
            }

            var strippedName = type.Name;
            if (type.Name.Contains("`"))
            {
                strippedName = type.Name.Substring(0, strippedName.IndexOf("`"));
            }
            name.Append(strippedName);

            if (type.GetGenericArguments().Length > 0)
            {
                name.Append("(");
                name.Append(string.Join(",", type.GetGenericArguments().Select(GetMEFContractName)));
                name.Append(")");
            }

            return name.ToString();
        }
    }
}