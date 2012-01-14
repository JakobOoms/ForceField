using System;
using Autofac;
using ForceField.AutofacIntegration;
using ForceField.Core.Advices;
using ForceField.Core.Pointcuts;
using ForceField.Examples.Advices;
using ForceField.Examples.CachingInfractructure;
using ForceField.Examples.Domain;
using ForceField.Examples.Services;
using ForceField.UnityIntegration;

namespace ForceField.Examples
{
    class Program
    {

        public class CacheConfigurationService : ICacheConfigurationService
        {
            public CacheConfiguration BuildConfiguration()
            {
                var fixedForOneHour = new CacheInstruction(CacheType.Fixed, TimeSpan.FromHours(1));
                var cacheConfig = new CacheConfiguration();
                cacheConfig.ApplyCachingOn(x => x.DeclaringType.Name.EndsWith("Repository"), fixedForOneHour)
                    .InvalidateOn(x => x.Name.StartsWith("Save"))
                    .InvalidateOn(x => x.Name.StartsWith("Delete"))
                    .InvalidateOn(x => x.Name.StartsWith("Update"));

                //apply exceptions
                //cacheConfig
                //    .ExceptFor<IPersonRepository>(x => x.GetByName(null))
                //    .InvalidateOn<IPersonRepository>(x => x.Save(null));
                return cacheConfig;
            }
        }


        static void Main(string[] args)
        {
            TestAutofacIntegration();
            TestUnityIntegration();
        }

        private static void TestUnityIntegration()
        {
            var unityAdvisorConfiguration = new UnityAdvisorConfiguration();
            unityAdvisorConfiguration.AddAdvice<CachingAdvice>(new AlwaysApplyAdvice());
            unityAdvisorConfiguration.AddAdvice<ExceptionHandlingAdvice>(new ApplyAdviceOnAllRepositories());
            unityAdvisorConfiguration.AddAdvice<LoggerAdvice>(new ApplyAdviceOnAllRepositories());
            unityAdvisorConfiguration.AddAdvice<EmptyAdvice>(ApplyAdvice.OnEveryMethod);
            var unity = new AOPEnabledUnityContainer(unityAdvisorConfiguration);
            unity.RegisterType<LoggerAdvice>();
            unity.RegisterType<CachingAdvice>();
            unity.RegisterType<ExceptionHandlingAdvice>();
            unity.RegisterType<ICacheProvider, RamCacheProvider>();
            unity.RegisterType<ILoggingService, LoggingService>();
            unity.RegisterType<IOtherService, OtherService>();
            unity.RegisterType<ICacheConfigurationService, CacheConfigurationService>();
            unity.RegisterType<IPersonRepository, PersonRepository>();


            var repository = unity.Resolve<IPersonRepository>();
            repository.Save(new Person { Key = 1, Age = 25, Name = "John" });
            repository.GetByName("John");
            repository.GetByName("John");

            var service = unity.Resolve<IOtherService>();
            var meaningOfLife = service.Bar();
        }

        private static void TestAutofacIntegration()
        {
            var advisorConfiguration = new AutofacAdvisorConfiguration();
            advisorConfiguration.AddAdvice<ExceptionHandlingAdvice>(new ApplyAdviceOnAllRepositories());
            advisorConfiguration.AddAdvice<LoggerAdvice>(new ApplyAdviceOnAllRepositories());
            advisorConfiguration.AddAdvice<CachingAdvice>(new ApplyAdviceOnAllRepositories());
            advisorConfiguration.AddAdvice<EmptyAdvice>(ApplyAdvice.OnEveryMethod);

            var builder = new ContainerBuilder();
            builder.Register(c => new LoggerAdvice(c.Resolve<ILoggingService>()));
            builder.Register(c => new RamCacheProvider()).As<ICacheProvider>().SingleInstance();
            builder.Register(c => new CacheConfigurationService()).As<ICacheConfigurationService>().SingleInstance();
            builder.Register(c => new CachingAdvice(c.Resolve<ICacheProvider>(), c.Resolve<ICacheConfigurationService>()));
            builder.Register(c => new ExceptionHandlingAdvice(c.Resolve<ILoggingService>()));
            builder.Register(c => new LoggingService()).As<ILoggingService>();
            builder.Register(c => new OtherService()).As<IOtherService>();
            builder.Register(c => new PersonRepository(c.Resolve<ILoggingService>())).As<IPersonRepository>().SingleInstance();

            using (var container = builder.Build(advisorConfiguration))
            {
                //var repository = container.Resolve<IPersonRepository>();
                //repository.Save(new Person { Key = 1, Age = 25, Name = "John" });
                //repository.GetByName("John");
                var service = container.Resolve<IOtherService>();
                var meaningOfLife = service.Bar();

                var otherInstance = container.Resolve<IPersonRepository>();
                otherInstance.Save(new Person { Age = 99, Name = "John" });
                otherInstance.GetByName("John");
                var john = otherInstance.GetByName("John");
            }
        }
    }
}
