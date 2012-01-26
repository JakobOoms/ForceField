using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using ForceField.Core;
using ForceField.Core.Advices;
using ForceField.TestUtils.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ForceField.AutofacIntegration.Tests
{
    [TestClass]
    public class ForceFieldAutofacContainerTests
    {
        [TestMethod]
        public void ImplementsIHaveConfiguration()
        {
            //Arrange
            var containerBuilder = new ContainerBuilder();
            var configuration = new Configuration();
            configuration.Add<TestAdvice>(ApplyAdvice.OnEveryMethod);

            //Act
            var forceFieldContainer = containerBuilder.Build(configuration);

            //Assert
            Assert.IsNotNull(forceFieldContainer as IHaveConfiguration);
        }

        [TestMethod]
        public void ResolveComponentReturnsAProxy()
        {
            //Arrange
            var containerBuilder = new ContainerBuilder();
            containerBuilder.Register(c => new TestInterfaceExtended()).As<ITestInterface>();
            var configuration = new Configuration();
            configuration.Add<TestAdvice>(ApplyAdvice.OnEveryMethod);
            var forceFieldContainer = containerBuilder.Build(configuration);

            //Act
            var resolvedInterface = forceFieldContainer.Resolve<ITestInterface>();

            //Assert
            Assert.IsNotNull(resolvedInterface as IDynamicProxy);
        }
    }
}
