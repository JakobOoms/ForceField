using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using ForceField.Core;
using ForceField.Core.Advices;
using ForceField.TestUtils.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ForceField.UnityIntegration.Tests
{
    [TestClass]
    public class ContainerTests
    {
        private ForceFieldUnityContainer CreateContainer()
        {
            var unityAdvisorConfiguration = new Configuration();
            unityAdvisorConfiguration.Add<TestAdvice>(ApplyAdvice.OnEveryMethod);
            var forceFieldUnityContainer = new ForceFieldUnityContainer(unityAdvisorConfiguration);
            forceFieldUnityContainer.RegisterType<ITestInterface, TestInterfaceExtended>();
            return forceFieldUnityContainer;
        }

        [TestMethod]
        public void ResolveNonGenericReturnsAProxy()
        {
            //Arrange
            var container = CreateContainer();

            //Act
            var resolvedInterface = container.Resolve(typeof(ITestInterface), "");

            //Assert
            Assert.IsNotNull(resolvedInterface as IDynamicProxy);
        }

        [TestMethod]
        public void ResolveGenericReturnsAProxy()
        {
            //Arrange
            var container = CreateContainer();

            //Act
            var resolvedInterface = container.Resolve<ITestInterface>();

            //Assert
            Assert.IsNotNull(resolvedInterface as IDynamicProxy);
        }

        [TestMethod]
        public void ResolveAllReturnsAProxy()
        {
            //Arrange
            var container = CreateContainer();

            //Act
            var resolvedInterfaces = container.ResolveAll(typeof(ITestInterface));

            //Assert
            foreach (var resolvedInterface in resolvedInterfaces)
            {
                Assert.IsNotNull(resolvedInterface as IDynamicProxy);
            }
        }
    }
}