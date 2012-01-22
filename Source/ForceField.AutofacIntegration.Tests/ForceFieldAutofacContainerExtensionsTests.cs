using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using ForceField.Core;
using ForceField.Core.Advices;
using ForceField.Core.Tests;
using ForceField.TestUtils.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ForceField.AutofacIntegration.Tests
{
    [TestClass]
    public class ForceFieldAutofacContainerExtensionsTests
    {
        [TestMethod]
        public void ContainerBuilderIsARequiredParameterForBuildMethod()
        {
            //Arrange
            ContainerBuilder builder = null;
            var config = new AutofacAdvisorConfiguration();

            //Act + Assert
            Expect.ArgumentNullException(() => builder.Build(config));
        }

        [TestMethod]
        public void ConfigurationIsARequiredParameterForBuildMethod()
        {
            //Arrange
            var builder = new ContainerBuilder();

            //Act + Assert
            Expect.ArgumentNullException(() => builder.Build(null));
        }

        [TestMethod]
        public void AllAdvicesShouldBeAutomaticlyRegistered()
        {
            //Arrange
            var containerBuilder = new ContainerBuilder();
            var configuration = new AutofacAdvisorConfiguration();
            configuration.AddAdvice<TestAdvice>(ApplyAdvice.OnEveryMethod);

            //Act
            var forceFieldContainer = containerBuilder.Build(configuration);
            var resolvedAdvice = forceFieldContainer.Resolve<TestAdvice>();

            //Assert
            Assert.IsNotNull(resolvedAdvice);
        }
    }
}
