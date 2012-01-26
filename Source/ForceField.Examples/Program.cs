using System;
using Autofac;
//using ForceField.AutofacIntegration;
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
        static void Main(string[] args)
        {
            //TestAutofacIntegration();
            TestUnityIntegration();
        }

        private static void TestUnityIntegration()
        {
            var forceFieldConfiguration = new Configuration();
            forceFieldConfiguration.Add<CachingAdvice>(ApplyAdvice.OnEveryMethod);
            forceFieldConfiguration.Add<ExceptionHandlingAdvice>(new ApplyAdviceOnAllRepositories());
            forceFieldConfiguration.Add<LoggerAdvice>(new ApplyAdviceOnAllRepositories());
            forceFieldConfiguration.Add<EmptyAdvice>(ApplyAdvice.OnEveryMethod);

            var unity = new ForceFieldUnityContainer(forceFieldConfiguration);
            unity.RegisterType<ICacheProvider, RamCacheProvider>();
            unity.RegisterType<ILoggingService, LoggingService>();
            unity.RegisterType<IOtherService, OtherService>();
            unity.RegisterType<ICacheConfigurationService, ExampleOfCacheConfigurationService>();
            unity.RegisterType<IPersonRepository, PersonRepository>();

            var repository = unity.Resolve<IPersonRepository>();
            repository.Save(new Person { Key = 1, Age = 25, Name = "John" });
            repository.GetByName("John");
            repository.GetByName("John");

            var service = unity.Resolve<IOtherService>();
            var meaningOfLife = service.Bar();
        }

        //private static void TestAutofacIntegration()
        //{
        //    var advisorConfiguration = new Configuration();
        //    advisorConfiguration.Add<ExceptionHandlingAdvice>(new ApplyAdviceOnAllRepositories());
        //    advisorConfiguration.Add<LoggerAdvice>(new ApplyAdviceOnAllRepositories());
        //    advisorConfiguration.Add<CachingAdvice>(new ApplyAdviceOnAllRepositories());
        //    advisorConfiguration.Add<EmptyAdvice>(ApplyAdvice.OnEveryMethod);

        //    var builder = new ContainerBuilder();
        //    builder.Register(c => new RamCacheProvider()).As<ICacheProvider>().SingleInstance();
        //    builder.Register(c => new ExampleOfCacheConfigurationService()).As<ICacheConfigurationService>().SingleInstance();
        //    builder.Register(c => new ExceptionHandlingAdvice(c.Resolve<ILoggingService>()));
        //    builder.Register(c => new LoggingService()).As<ILoggingService>();
        //    builder.Register(c => new OtherService()).As<IOtherService>();
        //    builder.Register(c => new PersonRepository(c.Resolve<ILoggingService>())).As<IPersonRepository>().SingleInstance();

        //    using (var container = builder.Build(advisorConfiguration))
        //    {
        //        //var repository = container.Resolve<IPersonRepository>();
        //        //repository.Save(new Person { Key = 1, Age = 25, Name = "John" });
        //        //repository.GetByName("John");
        //        var service = container.Resolve<IOtherService>();
        //        var meaningOfLife = service.Bar();

        //        var otherInstance = container.Resolve<IPersonRepository>();
        //        otherInstance.Save(new Person { Age = 99, Name = "John" });
        //        otherInstance.GetByName("John");
        //        var john = otherInstance.GetByName("John");
        //    }
        //}
    }
}
