using System;
using System.Collections.Generic;
using System.Linq;
using ForceField.Core;
using Microsoft.Practices.Unity;

namespace ForceField.UnityIntegration
{
    public class ForceFieldUnityContainer : IUnityContainer
    {
        private readonly Configuration _configuration;
        private readonly UnityContainer _innerContainer;

        public ForceFieldUnityContainer(Configuration configuration)
        {
            Guard.ArgumentIsNotNull(() => configuration);

            _configuration = configuration;
            _configuration.SetInnerContainer(this);
            _innerContainer = new UnityContainer();

            //Register all the advices into the container, so they can be resolved when needed
            foreach (var adviceType in configuration.GetRegisteredAdvices())
                _innerContainer.RegisterType(adviceType);
        }

        public BaseConfiguration Configuration
        {
            get { return _configuration; }
        }

        public IUnityContainer AddExtension(UnityContainerExtension extension)
        {
            return _innerContainer.AddExtension(extension);
        }

        public object BuildUp(Type t, object existing, string name, params ResolverOverride[] resolverOverrides)
        {
            return _innerContainer.BuildUp(t, existing, name, resolverOverrides);
        }

        public object Configure(Type configurationInterface)
        {
            return _innerContainer.Configure(configurationInterface);
        }

        public IUnityContainer CreateChildContainer()
        {
            return _innerContainer.CreateChildContainer();
        }

        public IUnityContainer Parent
        {
            get { return _innerContainer.Parent; }
        }

        public IUnityContainer RegisterInstance(Type t, string name, object instance, LifetimeManager lifetime)
        {
            _innerContainer.RegisterInstance(t, name, instance, lifetime);
            return this;
        }

        public IUnityContainer RegisterType(Type from, Type to, string name, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
        {
            _innerContainer.RegisterType(from, to, name, lifetimeManager, injectionMembers);
            return this;
        }

        public IEnumerable<ContainerRegistration> Registrations
        {
            get { return _innerContainer.Registrations; }
        }

        public IUnityContainer RemoveAllExtensions()
        {
            _innerContainer.RemoveAllExtensions();
            return this;
        }

        public object Resolve(Type t, string name, params ResolverOverride[] resolverOverrides)
        {
            var o = _innerContainer.Resolve(t, name, resolverOverrides);
            return ProxyFactory.Create(t, o, Configuration);
        }

        public IEnumerable<object> ResolveAll(Type t, params ResolverOverride[] resolverOverrides)
        {
            var resolvedItems = _innerContainer.ResolveAll(t, resolverOverrides);
            return resolvedItems.Select(implementation => ProxyFactory.Create(t, implementation, Configuration));
        }

        public void Teardown(object o)
        {
            _innerContainer.Teardown(o);
        }

        public void Dispose()
        {
            _innerContainer.Dispose();
        }

        public T Resolve<T>() where T : class
        {
            var innerService = _innerContainer.Resolve<T>();
            return ProxyFactory.Create(innerService, Configuration);
        }

        public void RegisterType<TFrom, TTo>() where TTo : TFrom
        {
            _innerContainer.RegisterType<TFrom, TTo>();
        }

        public void RegisterType<T>()
        {
            _innerContainer.RegisterType<T, T>();
        }
    }
}