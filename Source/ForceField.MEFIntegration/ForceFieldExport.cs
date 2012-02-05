using System;
using System.ComponentModel.Composition.Primitives;
using ForceField.Core;

namespace ForceField.MEFIntegration
{
    /// <summary>
    /// Export that creates a proxy when the actual value is retrieved from the export
    /// </summary>
    public class ForceFieldExport : Export
    {
        private readonly Configuration _config;
        private readonly Export _source;
        private readonly Type _typeToProxy;

        public ForceFieldExport(Configuration config, Export source, Type typeToProxy)
            : base(source.Definition, () => null)
        {
            _config = config;
            _source = source;
            _typeToProxy = typeToProxy;
        }

        protected override object GetExportedValueCore()
        {
            return ProxyFactory.Create(_typeToProxy, _source.Value, _config);
        }
    }
}