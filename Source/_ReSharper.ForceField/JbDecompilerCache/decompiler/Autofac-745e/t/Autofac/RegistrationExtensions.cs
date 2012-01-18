// Type: Autofac.RegistrationExtensions
// Assembly: Autofac, Version=2.5.2.830, Culture=neutral, PublicKeyToken=17863af14b0044da
// Assembly location: C:\_Git\Projects\ForceField\Source\packages\Autofac.2.5.2.830\lib\NET40\Autofac.dll

using Autofac.Builder;
using Autofac.Core;
using Autofac.Core.Activators.ProvidedInstance;
using Autofac.Core.Activators.Reflection;
using Autofac.Core.Lifetime;
using Autofac.Features.LightweightAdapters;
using Autofac.Features.OpenGenerics;
using Autofac.Features.Scanning;
using Autofac.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Autofac
{
  public static class RegistrationExtensions
  {
    public static void RegisterModule(this ContainerBuilder builder, IModule module)
    {
      if (builder == null)
        throw new ArgumentNullException("builder");
      if (module == null)
        throw new ArgumentNullException("module");
      builder.RegisterCallback(new Action<IComponentRegistry>(module.Configure));
    }

    public static void RegisterModule<TModule>(this ContainerBuilder builder) where TModule : new(), IModule
    {
      Autofac.RegistrationExtensions.RegisterModule(builder, (IModule) new TModule());
    }

    public static void RegisterComponent(this ContainerBuilder builder, IComponentRegistration registration)
    {
      if (builder == null)
        throw new ArgumentNullException("builder");
      if (registration == null)
        throw new ArgumentNullException("registration");
      builder.RegisterCallback((Action<IComponentRegistry>) (cr => cr.Register(registration)));
    }

    public static void RegisterSource(this ContainerBuilder builder, IRegistrationSource registrationSource)
    {
      if (builder == null)
        throw new ArgumentNullException("builder");
      if (registrationSource == null)
        throw new ArgumentNullException("registrationSource");
      builder.RegisterCallback((Action<IComponentRegistry>) (cr => cr.AddRegistrationSource(registrationSource)));
    }

    public static IRegistrationBuilder<T, SimpleActivatorData, SingleRegistrationStyle> RegisterInstance<T>(this ContainerBuilder builder, T instance) where T : class
    {
      if (builder == null)
        throw new ArgumentNullException("builder");
      if ((object) instance == null)
        throw new ArgumentNullException("instance");
      ProvidedInstanceActivator activator = new ProvidedInstanceActivator((object) instance);
      RegistrationBuilder<T, SimpleActivatorData, SingleRegistrationStyle> rb = new RegistrationBuilder<T, SimpleActivatorData, SingleRegistrationStyle>((Service) new TypedService(typeof (T)), new SimpleActivatorData((IInstanceActivator) activator), new SingleRegistrationStyle());
      rb.SingleInstance();
      builder.RegisterCallback((Action<IComponentRegistry>) (cr =>
      {
        if (!(rb.RegistrationData.Lifetime is RootScopeLifetime) || rb.RegistrationData.Sharing != InstanceSharing.Shared)
          throw new InvalidOperationException(string.Format(RegistrationExtensionsResources.InstanceRegistrationsAreSingleInstanceOnly, (object) (T) instance));
        activator.DisposeInstance = rb.RegistrationData.Ownership == InstanceOwnership.OwnedByLifetimeScope;
        RegistrationBuilder.RegisterSingleComponent<T, SimpleActivatorData, SingleRegistrationStyle>(cr, (IRegistrationBuilder<T, SimpleActivatorData, SingleRegistrationStyle>) rb);
      }));
      return (IRegistrationBuilder<T, SimpleActivatorData, SingleRegistrationStyle>) rb;
    }

    public static IRegistrationBuilder<TImplementor, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterType<TImplementor>(this ContainerBuilder builder)
    {
      if (builder == null)
        throw new ArgumentNullException("builder");
      IRegistrationBuilder<TImplementor, ConcreteReflectionActivatorData, SingleRegistrationStyle> rb = RegistrationBuilder.ForType<TImplementor>();
      builder.RegisterCallback((Action<IComponentRegistry>) (cr => RegistrationBuilder.RegisterSingleComponent<TImplementor, ConcreteReflectionActivatorData, SingleRegistrationStyle>(cr, rb)));
      return rb;
    }

    public static IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterType(this ContainerBuilder builder, Type implementationType)
    {
      if (builder == null)
        throw new ArgumentNullException("builder");
      if (implementationType == (Type) null)
        throw new ArgumentNullException("implementationType");
      IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> rb = RegistrationBuilder.ForType(implementationType);
      builder.RegisterCallback((Action<IComponentRegistry>) (cr => RegistrationBuilder.RegisterSingleComponent<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>(cr, rb)));
      return rb;
    }

    public static IRegistrationBuilder<T, SimpleActivatorData, SingleRegistrationStyle> Register<T>(this ContainerBuilder builder, Func<IComponentContext, T> @delegate)
    {
      if (@delegate == null)
        throw new ArgumentNullException("delegate");
      else
        return Autofac.RegistrationExtensions.Register<T>(builder, (Func<IComponentContext, IEnumerable<Parameter>, T>) ((c, p) => @delegate(c)));
    }

    public static IRegistrationBuilder<T, SimpleActivatorData, SingleRegistrationStyle> Register<T>(this ContainerBuilder builder, Func<IComponentContext, IEnumerable<Parameter>, T> @delegate)
    {
      if (builder == null)
        throw new ArgumentNullException("builder");
      if (@delegate == null)
        throw new ArgumentNullException("delegate");
      IRegistrationBuilder<T, SimpleActivatorData, SingleRegistrationStyle> rb = RegistrationBuilder.ForDelegate<T>(@delegate);
      builder.RegisterCallback((Action<IComponentRegistry>) (cr => RegistrationBuilder.RegisterSingleComponent<T, SimpleActivatorData, SingleRegistrationStyle>(cr, rb)));
      return rb;
    }

    public static IRegistrationBuilder<object, ReflectionActivatorData, DynamicRegistrationStyle> RegisterGeneric(this ContainerBuilder builder, Type implementor)
    {
      return OpenGenericRegistrationExtensions.RegisterGeneric(builder, implementor);
    }

    public static IRegistrationBuilder<TLimit, TActivatorData, TSingleRegistrationStyle> PreserveExistingDefaults<TLimit, TActivatorData, TSingleRegistrationStyle>(this IRegistrationBuilder<TLimit, TActivatorData, TSingleRegistrationStyle> registration) where TSingleRegistrationStyle : SingleRegistrationStyle
    {
      if (registration == null)
        throw new ArgumentNullException("registration");
      registration.RegistrationStyle.PreserveDefaults = true;
      return registration;
    }

    public static IRegistrationBuilder<TLimit, ScanningActivatorData, TRegistrationStyle> PreserveExistingDefaults<TLimit, TRegistrationStyle>(this IRegistrationBuilder<TLimit, ScanningActivatorData, TRegistrationStyle> registration)
    {
      return ScanningRegistrationExtensions.PreserveExistingDefaults<TLimit, ScanningActivatorData, TRegistrationStyle>(registration);
    }

    public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> RegisterAssemblyTypes(this ContainerBuilder builder, params Assembly[] assemblies)
    {
      return ScanningRegistrationExtensions.RegisterAssemblyTypes(builder, assemblies);
    }

    public static IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> Where<TLimit, TScanningActivatorData, TRegistrationStyle>(this IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> registration, Func<Type, bool> predicate) where TScanningActivatorData : ScanningActivatorData
    {
      if (registration == null)
        throw new ArgumentNullException("registration");
      registration.ActivatorData.Filters.Add(predicate);
      return registration;
    }

    public static IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> As<TLimit, TScanningActivatorData, TRegistrationStyle>(this IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> registration, Func<Type, IEnumerable<Service>> serviceMapping) where TScanningActivatorData : ScanningActivatorData
    {
      if (registration == null)
        throw new ArgumentNullException("registration");
      if (serviceMapping == null)
        throw new ArgumentNullException("serviceMapping");
      else
        return ScanningRegistrationExtensions.As<TLimit, TScanningActivatorData, TRegistrationStyle>(registration, serviceMapping);
    }

    public static IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> As<TLimit, TScanningActivatorData, TRegistrationStyle>(this IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> registration, Func<Type, Service> serviceMapping) where TScanningActivatorData : ScanningActivatorData
    {
      if (registration == null)
        throw new ArgumentNullException("registration");
      else
        return Autofac.RegistrationExtensions.As<TLimit, TScanningActivatorData, TRegistrationStyle>(registration, (Func<Type, IEnumerable<Service>>) (t => (IEnumerable<Service>) new Service[1]
        {
          serviceMapping(t)
        }));
    }

    public static IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> As<TLimit, TScanningActivatorData, TRegistrationStyle>(this IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> registration, Func<Type, Type> serviceMapping) where TScanningActivatorData : ScanningActivatorData
    {
      if (registration == null)
        throw new ArgumentNullException("registration");
      else
        return Autofac.RegistrationExtensions.As<TLimit, TScanningActivatorData, TRegistrationStyle>(registration, (Func<Type, Service>) (t => (Service) new TypedService(serviceMapping(t))));
    }

    public static IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> As<TLimit, TScanningActivatorData, TRegistrationStyle>(this IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> registration, Func<Type, IEnumerable<Type>> serviceMapping) where TScanningActivatorData : ScanningActivatorData
    {
      if (registration == null)
        throw new ArgumentNullException("registration");
      else
        return Autofac.RegistrationExtensions.As<TLimit, TScanningActivatorData, TRegistrationStyle>(registration, (Func<Type, IEnumerable<Service>>) (t => Enumerable.Select<Type, Service>(serviceMapping(t), (Func<Type, Service>) (s => (Service) new TypedService(s)))));
    }

    public static IRegistrationBuilder<TLimit, ScanningActivatorData, DynamicRegistrationStyle> AsSelf<TLimit>(this IRegistrationBuilder<TLimit, ScanningActivatorData, DynamicRegistrationStyle> registration)
    {
      if (registration == null)
        throw new ArgumentNullException("registration");
      else
        return Autofac.RegistrationExtensions.As<TLimit, ScanningActivatorData, DynamicRegistrationStyle>(registration, (Func<Type, Type>) (t => t));
    }

    public static IRegistrationBuilder<TLimit, TConcreteActivatorData, SingleRegistrationStyle> AsSelf<TLimit, TConcreteActivatorData>(this IRegistrationBuilder<TLimit, TConcreteActivatorData, SingleRegistrationStyle> registration) where TConcreteActivatorData : IConcreteActivatorData
    {
      if (registration == null)
        throw new ArgumentNullException("registration");
      return registration.As(new Type[1]
      {
        registration.ActivatorData.Activator.LimitType
      });
    }

    public static IRegistrationBuilder<TLimit, ReflectionActivatorData, DynamicRegistrationStyle> AsSelf<TLimit>(this IRegistrationBuilder<TLimit, ReflectionActivatorData, DynamicRegistrationStyle> registration)
    {
      if (registration == null)
        throw new ArgumentNullException("registration");
      return registration.As(new Type[1]
      {
        registration.ActivatorData.ImplementationType
      });
    }

    public static IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> WithMetadata<TLimit, TScanningActivatorData, TRegistrationStyle>(this IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> registration, Func<Type, IEnumerable<KeyValuePair<string, object>>> metadataMapping) where TScanningActivatorData : ScanningActivatorData
    {
      if (registration == null)
        throw new ArgumentNullException("registration");
      registration.ActivatorData.ConfigurationActions.Add((Action<Type, IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>>) ((t, rb) => rb.WithMetadata(metadataMapping(t))));
      return registration;
    }

    public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> WithMetadataFrom<TAttribute>(this IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> registration)
    {
      PropertyInfo[] metadataProperties = typeof (TAttribute).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);
      return Autofac.RegistrationExtensions.WithMetadata<object, ScanningActivatorData, DynamicRegistrationStyle>(registration, (Func<Type, IEnumerable<KeyValuePair<string, object>>>) (t =>
      {
        TAttribute[] local_0 = Enumerable.ToArray<TAttribute>(Enumerable.OfType<TAttribute>((IEnumerable) t.GetCustomAttributes(true)));
        if (local_0.Length == 0)
          throw new ArgumentException(string.Format("A metadata attribute of type {0} was not found on {1}.", (object) typeof (TAttribute), (object) t));
        if (local_0.Length != 1)
          throw new ArgumentException(string.Format("More than one metadata attribute of type {0} was found on {1}.", (object) typeof (TAttribute), (object) t));
        TAttribute attr = local_0[0];
        return Enumerable.Select<PropertyInfo, KeyValuePair<string, object>>((IEnumerable<PropertyInfo>) metadataProperties, (Func<PropertyInfo, KeyValuePair<string, object>>) (p => new KeyValuePair<string, object>(p.Name, p.GetValue((object) attr, (object[]) null))));
      }));
    }

    public static IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> WithMetadata<TLimit, TScanningActivatorData, TRegistrationStyle>(this IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> registration, string metadataKey, Func<Type, object> metadataValueMapping) where TScanningActivatorData : ScanningActivatorData
    {
      if (registration == null)
        throw new ArgumentNullException("registration");
      else
        return Autofac.RegistrationExtensions.WithMetadata<TLimit, TScanningActivatorData, TRegistrationStyle>(registration, (Func<Type, IEnumerable<KeyValuePair<string, object>>>) (t => (IEnumerable<KeyValuePair<string, object>>) new KeyValuePair<string, object>[1]
        {
          new KeyValuePair<string, object>(metadataKey, metadataValueMapping(t))
        }));
    }

    public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> Named<TService>(this IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> registration, Func<Type, string> serviceNameMapping)
    {
      return Autofac.RegistrationExtensions.Named<object, ScanningActivatorData, DynamicRegistrationStyle>(registration, serviceNameMapping, typeof (TService));
    }

    public static IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> Named<TLimit, TScanningActivatorData, TRegistrationStyle>(this IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> registration, Func<Type, string> serviceNameMapping, Type serviceType) where TScanningActivatorData : ScanningActivatorData
    {
      if (registration == null)
        throw new ArgumentNullException("registration");
      if (serviceNameMapping == null)
        throw new ArgumentNullException("serviceNameMapping");
      if (serviceType == (Type) null)
        throw new ArgumentNullException("serviceType");
      else
        return Autofac.RegistrationExtensions.As<TLimit, TScanningActivatorData, TRegistrationStyle>(registration, (Func<Type, Service>) (t => (Service) new KeyedService((object) serviceNameMapping(t), serviceType)));
    }

    public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> Keyed<TService>(this IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> registration, Func<Type, object> serviceKeyMapping)
    {
      return Autofac.RegistrationExtensions.Keyed<object, ScanningActivatorData, DynamicRegistrationStyle>(registration, serviceKeyMapping, typeof (TService));
    }

    public static IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> Keyed<TLimit, TScanningActivatorData, TRegistrationStyle>(this IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> registration, Func<Type, object> serviceKeyMapping, Type serviceType) where TScanningActivatorData : ScanningActivatorData
    {
      if (registration == null)
        throw new ArgumentNullException("registration");
      if (serviceKeyMapping == null)
        throw new ArgumentNullException("serviceKeyMapping");
      if (serviceType == (Type) null)
        throw new ArgumentNullException("serviceType");
      else
        return Autofac.RegistrationExtensions.As<TLimit, TScanningActivatorData, TRegistrationStyle>(Autofac.RegistrationExtensions.AssignableTo<TLimit, TScanningActivatorData, TRegistrationStyle>(registration, serviceType), (Func<Type, Service>) (t => (Service) new KeyedService(serviceKeyMapping(t), serviceType)));
    }

    public static IRegistrationBuilder<TLimit, ScanningActivatorData, DynamicRegistrationStyle> AsImplementedInterfaces<TLimit>(this IRegistrationBuilder<TLimit, ScanningActivatorData, DynamicRegistrationStyle> registration)
    {
      if (registration == null)
        throw new ArgumentNullException("registration");
      else
        return Autofac.RegistrationExtensions.As<TLimit, ScanningActivatorData, DynamicRegistrationStyle>(registration, (Func<Type, IEnumerable<Type>>) (t => (IEnumerable<Type>) Autofac.RegistrationExtensions.GetImplementedInterfaces(t)));
    }

    public static IRegistrationBuilder<TLimit, TConcreteActivatorData, SingleRegistrationStyle> AsImplementedInterfaces<TLimit, TConcreteActivatorData>(this IRegistrationBuilder<TLimit, TConcreteActivatorData, SingleRegistrationStyle> registration) where TConcreteActivatorData : IConcreteActivatorData
    {
      if (registration == null)
        throw new ArgumentNullException("registration");
      else
        return registration.As(Autofac.RegistrationExtensions.GetImplementedInterfaces(registration.ActivatorData.Activator.LimitType));
    }

    public static IRegistrationBuilder<TLimit, ReflectionActivatorData, DynamicRegistrationStyle> AsImplementedInterfaces<TLimit>(this IRegistrationBuilder<TLimit, ReflectionActivatorData, DynamicRegistrationStyle> registration)
    {
      if (registration == null)
        throw new ArgumentNullException("registration");
      Type implementationType = registration.ActivatorData.ImplementationType;
      return registration.As(Autofac.RegistrationExtensions.GetImplementedInterfaces(implementationType));
    }

    private static Type[] GetImplementedInterfaces(Type type)
    {
      return Enumerable.ToArray<Type>(Enumerable.Where<Type>((IEnumerable<Type>) type.GetInterfaces(), (Func<Type, bool>) (i => i != typeof (IDisposable))));
    }

    public static IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> FindConstructorsWith<TLimit, TReflectionActivatorData, TStyle>(this IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> registration, BindingFlags bindingFlags) where TReflectionActivatorData : ReflectionActivatorData
    {
      if (registration == null)
        throw new ArgumentNullException("registration");
      else
        return Autofac.RegistrationExtensions.FindConstructorsWith<TLimit, TReflectionActivatorData, TStyle>(registration, (IConstructorFinder) new BindingFlagsConstructorFinder(bindingFlags));
    }

    public static IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> FindConstructorsWith<TLimit, TReflectionActivatorData, TStyle>(this IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> registration, IConstructorFinder constructorFinder) where TReflectionActivatorData : ReflectionActivatorData
    {
      if (registration == null)
        throw new ArgumentNullException("registration");
      if (constructorFinder == null)
        throw new ArgumentNullException("constructorFinder");
      registration.ActivatorData.ConstructorFinder = constructorFinder;
      return registration;
    }

    public static IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> UsingConstructor<TLimit, TReflectionActivatorData, TStyle>(this IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> registration, params Type[] signature) where TReflectionActivatorData : ReflectionActivatorData
    {
      if (registration == null)
        throw new ArgumentNullException("registration");
      if (signature == null)
        throw new ArgumentNullException("signature");
      if (registration.ActivatorData.ImplementationType.GetConstructor(signature) == (ConstructorInfo) null)
        throw new ArgumentException(string.Format(RegistrationExtensionsResources.NoMatchingConstructorExists, (object) registration.ActivatorData.ImplementationType));
      else
        return Autofac.RegistrationExtensions.UsingConstructor<TLimit, TReflectionActivatorData, TStyle>(registration, (IConstructorSelector) new MatchingSignatureConstructorSelector(signature));
    }

    public static IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> UsingConstructor<TLimit, TReflectionActivatorData, TStyle>(this IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> registration, IConstructorSelector constructorSelector) where TReflectionActivatorData : ReflectionActivatorData
    {
      if (registration == null)
        throw new ArgumentNullException("registration");
      if (constructorSelector == null)
        throw new ArgumentNullException("constructorSelector");
      registration.ActivatorData.ConstructorSelector = constructorSelector;
      return registration;
    }

    public static IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> WithParameter<TLimit, TReflectionActivatorData, TStyle>(this IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> registration, string parameterName, object parameterValue) where TReflectionActivatorData : ReflectionActivatorData
    {
      return Autofac.RegistrationExtensions.WithParameter<TLimit, TReflectionActivatorData, TStyle>(registration, (Parameter) new NamedParameter(parameterName, parameterValue));
    }

    public static IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> WithParameter<TLimit, TReflectionActivatorData, TStyle>(this IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> registration, Parameter parameter) where TReflectionActivatorData : ReflectionActivatorData
    {
      if (registration == null)
        throw new ArgumentNullException("registration");
      if (parameter == null)
        throw new ArgumentNullException("parameter");
      registration.ActivatorData.ConfiguredParameters.Add(parameter);
      return registration;
    }

    public static IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> WithParameter<TLimit, TReflectionActivatorData, TStyle>(this IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> registration, Func<ParameterInfo, IComponentContext, bool> parameterSelector, Func<ParameterInfo, IComponentContext, object> valueProvider) where TReflectionActivatorData : ReflectionActivatorData
    {
      if (parameterSelector == null)
        throw new ArgumentNullException("parameterSelector");
      if (valueProvider == null)
        throw new ArgumentNullException("valueProvider");
      else
        return Autofac.RegistrationExtensions.WithParameter<TLimit, TReflectionActivatorData, TStyle>(registration, (Parameter) new ResolvedParameter(parameterSelector, valueProvider));
    }

    public static IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> WithParameters<TLimit, TReflectionActivatorData, TStyle>(this IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> registration, IEnumerable<Parameter> parameters) where TReflectionActivatorData : ReflectionActivatorData
    {
      if (parameters == null)
        throw new ArgumentNullException("parameters");
      foreach (Parameter parameter in parameters)
        Autofac.RegistrationExtensions.WithParameter<TLimit, TReflectionActivatorData, TStyle>(registration, parameter);
      return registration;
    }

    public static IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> WithProperty<TLimit, TReflectionActivatorData, TStyle>(this IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> registration, string propertyName, object propertyValue) where TReflectionActivatorData : ReflectionActivatorData
    {
      return Autofac.RegistrationExtensions.WithProperty<TLimit, TReflectionActivatorData, TStyle>(registration, (Parameter) new NamedPropertyParameter(propertyName, propertyValue));
    }

    public static IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> WithProperty<TLimit, TReflectionActivatorData, TStyle>(this IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> registration, Parameter property) where TReflectionActivatorData : ReflectionActivatorData
    {
      if (registration == null)
        throw new ArgumentNullException("registration");
      if (property == null)
        throw new ArgumentNullException("property");
      registration.ActivatorData.ConfiguredProperties.Add(property);
      return registration;
    }

    public static IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> WithProperties<TLimit, TReflectionActivatorData, TStyle>(this IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> registration, IEnumerable<Parameter> properties) where TReflectionActivatorData : ReflectionActivatorData
    {
      if (registration == null)
        throw new ArgumentNullException("registration");
      if (properties == null)
        throw new ArgumentNullException("properties");
      foreach (Parameter property in properties)
        Autofac.RegistrationExtensions.WithProperty<TLimit, TReflectionActivatorData, TStyle>(registration, property);
      return registration;
    }

    public static IRegistrationBuilder<TLimit, TActivatorData, TSingleRegistrationStyle> Targeting<TLimit, TActivatorData, TSingleRegistrationStyle>(this IRegistrationBuilder<TLimit, TActivatorData, TSingleRegistrationStyle> registration, IComponentRegistration target) where TSingleRegistrationStyle : SingleRegistrationStyle
    {
      if (target == null)
        throw new ArgumentNullException("target");
      registration.RegistrationStyle.Target = target.Target;
      return registration;
    }

    public static IRegistrationBuilder<TLimit, TActivatorData, TSingleRegistrationStyle> OnRegistered<TLimit, TActivatorData, TSingleRegistrationStyle>(this IRegistrationBuilder<TLimit, TActivatorData, TSingleRegistrationStyle> registration, Action<ComponentRegisteredEventArgs> handler) where TSingleRegistrationStyle : SingleRegistrationStyle
    {
      if (registration == null)
        throw new ArgumentNullException("registration");
      if (handler == null)
        throw new ArgumentNullException("handler");
      registration.RegistrationStyle.RegisteredHandlers.Add((EventHandler<ComponentRegisteredEventArgs>) ((s, e) => handler(e)));
      return registration;
    }

    public static IRegistrationBuilder<TLimit, ScanningActivatorData, TRegistrationStyle> OnRegistered<TLimit, TRegistrationStyle>(this IRegistrationBuilder<TLimit, ScanningActivatorData, TRegistrationStyle> registration, Action<ComponentRegisteredEventArgs> handler)
    {
      if (registration == null)
        throw new ArgumentNullException("registration");
      if (handler == null)
        throw new ArgumentNullException("handler");
      registration.ActivatorData.ConfigurationActions.Add((Action<Type, IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>>) ((t, rb) => Autofac.RegistrationExtensions.OnRegistered<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>(rb, handler)));
      return registration;
    }

    public static IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> AsClosedTypesOf<TLimit, TScanningActivatorData, TRegistrationStyle>(this IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> registration, Type openGenericServiceType) where TScanningActivatorData : ScanningActivatorData
    {
      return ScanningRegistrationExtensions.AsClosedTypesOf<TLimit, TScanningActivatorData, TRegistrationStyle>(registration, openGenericServiceType);
    }

    public static IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> AssignableTo<TLimit, TScanningActivatorData, TRegistrationStyle>(this IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> registration, Type type) where TScanningActivatorData : ScanningActivatorData
    {
      return ScanningRegistrationExtensions.AssignableTo<TLimit, TScanningActivatorData, TRegistrationStyle>(registration, type);
    }

    public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> AssignableTo<T>(this IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> registration)
    {
      return Autofac.RegistrationExtensions.AssignableTo<object, ScanningActivatorData, DynamicRegistrationStyle>(registration, typeof (T));
    }

    public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> Except<T>(this IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> registration)
    {
      return Autofac.RegistrationExtensions.Where<object, ScanningActivatorData, DynamicRegistrationStyle>(registration, (Func<Type, bool>) (t => t != typeof (T)));
    }

    public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> Except<T>(this IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> registration, Action<IRegistrationBuilder<T, ConcreteReflectionActivatorData, SingleRegistrationStyle>> customisedRegistration)
    {
      IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> registrationBuilder = Autofac.RegistrationExtensions.Except<T>(registration);
      registrationBuilder.ActivatorData.PostScanningCallbacks.Add((Action<IComponentRegistry>) (cr =>
      {
        IRegistrationBuilder<T, ConcreteReflectionActivatorData, SingleRegistrationStyle> local_0 = RegistrationBuilder.ForType<T>();
        customisedRegistration(local_0);
        RegistrationBuilder.RegisterSingleComponent<T, ConcreteReflectionActivatorData, SingleRegistrationStyle>(cr, local_0);
      }));
      return registrationBuilder;
    }

    public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> InNamespaceOf<T>(this IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> registration)
    {
      if (registration == null)
        throw new ArgumentNullException("registration");
      else
        return Autofac.RegistrationExtensions.InNamespace<object, ScanningActivatorData, DynamicRegistrationStyle>(registration, typeof (T).Namespace);
    }

    public static IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> InNamespace<TLimit, TScanningActivatorData, TRegistrationStyle>(this IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> registration, string ns) where TScanningActivatorData : ScanningActivatorData
    {
      if (registration == null)
        throw new ArgumentNullException("registration");
      if (ns == null)
        throw new ArgumentNullException("ns");
      else
        return Autofac.RegistrationExtensions.Where<TLimit, TScanningActivatorData, TRegistrationStyle>(registration, (Func<Type, bool>) (t => Autofac.TypeExtensions.IsInNamespace(t, ns)));
    }

    public static IRegistrationBuilder<TTo, LightweightAdapterActivatorData, DynamicRegistrationStyle> RegisterAdapter<TFrom, TTo>(this ContainerBuilder builder, Func<IComponentContext, IEnumerable<Parameter>, TFrom, TTo> adapter)
    {
      if (builder == null)
        throw new ArgumentNullException("builder");
      if (adapter == null)
        throw new ArgumentNullException("adapter");
      else
        return LightweightAdapterRegistrationExtensions.RegisterAdapter<TFrom, TTo>(builder, adapter);
    }

    public static IRegistrationBuilder<TTo, LightweightAdapterActivatorData, DynamicRegistrationStyle> RegisterAdapter<TFrom, TTo>(this ContainerBuilder builder, Func<IComponentContext, TFrom, TTo> adapter)
    {
      if (builder == null)
        throw new ArgumentNullException("builder");
      if (adapter == null)
        throw new ArgumentNullException("adapter");
      else
        return Autofac.RegistrationExtensions.RegisterAdapter<TFrom, TTo>(builder, (Func<IComponentContext, IEnumerable<Parameter>, TFrom, TTo>) ((c, p, f) => adapter(c, f)));
    }

    public static IRegistrationBuilder<TTo, LightweightAdapterActivatorData, DynamicRegistrationStyle> RegisterAdapter<TFrom, TTo>(this ContainerBuilder builder, Func<TFrom, TTo> adapter)
    {
      if (builder == null)
        throw new ArgumentNullException("builder");
      if (adapter == null)
        throw new ArgumentNullException("adapter");
      else
        return Autofac.RegistrationExtensions.RegisterAdapter<TFrom, TTo>(builder, (Func<IComponentContext, IEnumerable<Parameter>, TFrom, TTo>) ((c, p, f) => adapter(f)));
    }

    public static IRegistrationBuilder<object, OpenGenericDecoratorActivatorData, DynamicRegistrationStyle> RegisterGenericDecorator(this ContainerBuilder builder, Type decoratorType, Type decoratedServiceType, object fromKey, object toKey = null)
    {
      if (builder == null)
        throw new ArgumentNullException("builder");
      if (decoratorType == (Type) null)
        throw new ArgumentNullException("decoratorType");
      if (decoratedServiceType == (Type) null)
        throw new ArgumentNullException("decoratedServiceType");
      else
        return OpenGenericRegistrationExtensions.RegisterGenericDecorator(builder, decoratorType, decoratedServiceType, fromKey, toKey);
    }

    public static IRegistrationBuilder<TService, LightweightAdapterActivatorData, DynamicRegistrationStyle> RegisterDecorator<TService>(this ContainerBuilder builder, Func<IComponentContext, IEnumerable<Parameter>, TService, TService> decorator, object fromKey, object toKey = null)
    {
      if (builder == null)
        throw new ArgumentNullException("builder");
      if (decorator == null)
        throw new ArgumentNullException("decorator");
      else
        return LightweightAdapterRegistrationExtensions.RegisterDecorator<TService>(builder, decorator, fromKey, toKey);
    }

    public static IRegistrationBuilder<TService, LightweightAdapterActivatorData, DynamicRegistrationStyle> RegisterDecorator<TService>(this ContainerBuilder builder, Func<IComponentContext, TService, TService> decorator, object fromKey, object toKey = null)
    {
      if (builder == null)
        throw new ArgumentNullException("builder");
      if (decorator == null)
        throw new ArgumentNullException("decorator");
      else
        return LightweightAdapterRegistrationExtensions.RegisterDecorator<TService>(builder, (Func<IComponentContext, IEnumerable<Parameter>, TService, TService>) ((c, p, f) => decorator(c, f)), fromKey, toKey);
    }

    public static IRegistrationBuilder<TService, LightweightAdapterActivatorData, DynamicRegistrationStyle> RegisterDecorator<TService>(this ContainerBuilder builder, Func<TService, TService> decorator, object fromKey, object toKey = null)
    {
      if (builder == null)
        throw new ArgumentNullException("builder");
      if (decorator == null)
        throw new ArgumentNullException("decorator");
      else
        return LightweightAdapterRegistrationExtensions.RegisterDecorator<TService>(builder, (Func<IComponentContext, IEnumerable<Parameter>, TService, TService>) ((c, p, f) => decorator(f)), fromKey, toKey);
    }

    public static IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> OnRelease<TLimit, TActivatorData, TRegistrationStyle>(this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> registration, Action<TLimit> releaseAction)
    {
      if (registration == null)
        throw new ArgumentNullException("registration");
      if (releaseAction == null)
        throw new ArgumentNullException("releaseAction");
      else
        return registration.ExternallyOwned().OnActivating((Action<IActivatingEventArgs<TLimit>>) (e =>
        {
          ReleaseAction local_0 = new ReleaseAction((Action) (() => releaseAction(e.Instance)));
          ResolutionExtensions.Resolve<ILifetimeScope>(e.Context).Disposer.AddInstanceForDisposal((IDisposable) local_0);
        }));
    }
  }
}
