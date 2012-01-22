// Type: Autofac.ContainerBuilder
// Assembly: Autofac, Version=2.5.2.830, Culture=neutral, PublicKeyToken=17863af14b0044da
// Assembly location: C:\_Git\Projects\ForceField\Source\packages\Autofac.2.5.2.830\lib\NET40\Autofac.dll

using Autofac.Builder;
using Autofac.Core;
using Autofac.Features.Collections;
using Autofac.Features.GeneratedFactories;
using Autofac.Features.Indexed;
using Autofac.Features.LazyDependencies;
using Autofac.Features.Metadata;
using Autofac.Features.OwnedInstances;
using Autofac.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Autofac
{
  public class ContainerBuilder
  {
    private readonly IList<Action<IComponentRegistry>> _configurationCallbacks = (IList<Action<IComponentRegistry>>) new List<Action<IComponentRegistry>>();
    private bool _wasBuilt;

    public virtual void RegisterCallback(Action<IComponentRegistry> configurationCallback)
    {
      this._configurationCallbacks.Add(Enforce.ArgumentNotNull<Action<IComponentRegistry>>(configurationCallback, "configurationCallback"));
    }

    public IContainer Build(ContainerBuildOptions options = ContainerBuildOptions.None)
    {
      Container container = new Container();
      this.Build(container.ComponentRegistry, (options & ContainerBuildOptions.ExcludeDefaultModules) != ContainerBuildOptions.None);
      if ((options & ContainerBuildOptions.IgnoreStartableComponents) == ContainerBuildOptions.None)
        ContainerBuilder.StartStartableComponents((IComponentContext) container);
      return (IContainer) container;
    }

    private static void StartStartableComponents(IComponentContext componentContext)
    {
      foreach (IComponentRegistration registration in componentContext.ComponentRegistry.RegistrationsFor((Service) new TypedService(typeof (IStartable))))
        ((IStartable) componentContext.ResolveComponent(registration, Enumerable.Empty<Parameter>())).Start();
    }

    public void Update(IContainer container)
    {
      if (container == null)
        throw new ArgumentNullException("container");
      this.Update(container.ComponentRegistry);
    }

    public void Update(IComponentRegistry componentRegistry)
    {
      if (componentRegistry == null)
        throw new ArgumentNullException("componentRegistry");
      this.Build(componentRegistry, true);
    }

    private void Build(IComponentRegistry componentRegistry, bool excludeDefaultModules)
    {
      if (componentRegistry == null)
        throw new ArgumentNullException("componentRegistry");
      if (this._wasBuilt)
        throw new InvalidOperationException(ContainerBuilderResources.BuildCanOnlyBeCalledOnce);
      this._wasBuilt = true;
      if (!excludeDefaultModules)
        this.RegisterDefaultAdapters(componentRegistry);
      foreach (Action<IComponentRegistry> action in (IEnumerable<Action<IComponentRegistry>>) this._configurationCallbacks)
        action(componentRegistry);
    }

    private void RegisterDefaultAdapters(IComponentRegistry componentRegistry)
    {
      Autofac.RegistrationExtensions.RegisterGeneric(this, typeof (KeyedServiceIndex<,>)).As(new Type[1]
      {
        typeof (IIndex<,>)
      }).InstancePerLifetimeScope();
      componentRegistry.AddRegistrationSource((IRegistrationSource) new CollectionRegistrationSource());
      componentRegistry.AddRegistrationSource((IRegistrationSource) new OwnedInstanceRegistrationSource());
      componentRegistry.AddRegistrationSource((IRegistrationSource) new MetaRegistrationSource());
      componentRegistry.AddRegistrationSource((IRegistrationSource) new LazyRegistrationSource());
      componentRegistry.AddRegistrationSource((IRegistrationSource) new LazyWithMetadataRegistrationSource());
      componentRegistry.AddRegistrationSource((IRegistrationSource) new StronglyTypedMetaRegistrationSource());
      componentRegistry.AddRegistrationSource((IRegistrationSource) new GeneratedFactoryRegistrationSource());
    }
  }
}
