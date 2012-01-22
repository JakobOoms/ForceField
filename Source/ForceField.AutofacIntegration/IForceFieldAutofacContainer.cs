using Autofac;
using ForceField.Core;

namespace ForceField.AutofacIntegration
{
    public interface IForceFieldAutofacContainer : IContainer, IHaveConfiguration
    {
    }
}