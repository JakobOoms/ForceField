using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Core;
using ForceField.Core;

namespace ForceField.AutofacIntegration
{
    public class ForceFieldAutofacContainer : IForceFieldAutofacContainer
    {
        private readonly IContainer _innerContainer;
        private readonly AdvisorsConfiguration _configuration;

        public ForceFieldAutofacContainer(IContainer innerContainer, AdvisorsConfiguration configuration)
        {
            _innerContainer = innerContainer;
            _configuration = configuration;
            innerContainer.ChildLifetimeScopeBeginning += (sender, e) => { if (ChildLifetimeScopeBeginning != null) ChildLifetimeScopeBeginning(sender, e); };
            innerContainer.CurrentScopeEnding += (sender, e) => { if (CurrentScopeEnding != null) CurrentScopeEnding(sender, e); };
            innerContainer.ResolveOperationBeginning += (sender, e) => { if (ResolveOperationBeginning != null) ResolveOperationBeginning(sender, e); };
        }

        public ILifetimeScope BeginLifetimeScope(object tag, Action<ContainerBuilder> configurationAction)
        {
            return _innerContainer.BeginLifetimeScope(tag, configurationAction);
        }

        public ILifetimeScope BeginLifetimeScope(Action<ContainerBuilder> configurationAction)
        {
            return _innerContainer.BeginLifetimeScope(configurationAction);
        }

        public ILifetimeScope BeginLifetimeScope(object tag)
        {
            return _innerContainer.BeginLifetimeScope(tag);
        }

        public ILifetimeScope BeginLifetimeScope()
        {
            return _innerContainer.BeginLifetimeScope();
        }

        public event EventHandler<Autofac.Core.Lifetime.LifetimeScopeBeginningEventArgs> ChildLifetimeScopeBeginning;

        public event EventHandler<Autofac.Core.Lifetime.LifetimeScopeEndingEventArgs> CurrentScopeEnding;

        public IDisposer Disposer
        {
            get { return _innerContainer.Disposer; }
        }

        public event EventHandler<Autofac.Core.Resolving.ResolveOperationBeginningEventArgs> ResolveOperationBeginning;

        public object Tag
        {
            get { return _innerContainer.Tag; }
        }

        public IComponentRegistry ComponentRegistry
        {
            get { return _innerContainer.ComponentRegistry; }
        }

        public object ResolveComponent(IComponentRegistration registration, IEnumerable<Parameter> parameters)
        {
            var instance = _innerContainer.ResolveComponent(registration, parameters);
            var typedService = (TypedService) registration.Services.First();
            return ProxyFactory.Create(typedService.ServiceType, instance, _configuration);
        }

        public void Dispose()
        {
            _innerContainer.Dispose();
        }

        public AdvisorsConfiguration Configuration
        {
            get { return _configuration; }
        }
    }
}