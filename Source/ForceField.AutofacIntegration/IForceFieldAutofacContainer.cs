using Autofac;
using ForceField.Core;

namespace ForceField.AutofacIntegration
{
    //TODO: keep / remove this interface? Does it have any added value?
    public interface IForceFieldAutofacContainer : IContainer, IHaveConfiguration
    {
    }
}