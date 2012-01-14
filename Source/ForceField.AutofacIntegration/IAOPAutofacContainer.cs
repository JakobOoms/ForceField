using Autofac;
using ForceField.Core;

namespace ForceField.AutofacIntegration
{
    public interface IAOPAutofacContainer : IContainer
    {
        AdvisorsConfiguration Configuration { get; }
    }
}