using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using ForceField.Core.Advices;
using ForceField.Core.Tests;
using ForceField.TestUtils.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ForceField.UnityIntegration.Tests
{
    [TestClass]
    public class ConfigurationTests
    {
        [TestMethod]
        public void InnerContainerIsARequiredParameterForConstructor()
        {
            //Arrange
            Configuration configuration = null;

            //Act + Assert
            Expect.ArgumentNullException(() => new ForceFieldUnityContainer(configuration));
        }

        [TestMethod]
        public void AllAdvicesShouldBeAutomaticlyRegistered()
        {
            //Arrange
            var configuration = new Configuration();
            configuration.Add<TestAdvice>(ApplyAdvice.OnEveryMethod);
            var container = new ForceFieldUnityContainer(configuration);

            //Act
            var resolvedAdvice = container.Resolve<TestAdvice>();

            //Assert
            Assert.IsNotNull(resolvedAdvice);
        }
    }
}
