using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ForceField.Core.Generator;
using ForceField.TestUtils.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ForceField.Core.Tests.Generator
{
    //TODO use Roslyn analyze methods to validate generated code instead of string comparison....
    [TestClass]
    public class ProxyGeneratorTest
    {
        [TestMethod]
        public void TypeIsRequiredWhenGeneratingProxy()
        {
            //Arrange
            var generator = new ProxyGenerator();

            //Act + Assert
            Expect.ArgumentNullException(() => generator.Generate(null));
        }

        [TestMethod]
        public void VoidIsNotAValidTypeWhenGeneratingProxy()
        {
            //Arrange
            var generator = new ProxyGenerator();

            //Act + Assert
            Expect.ArgumentOutOfRangeException(() => generator.Generate(typeof(void)));
        }

        [TestMethod]
        public void MethodsFromDerivedInterfacesShouldAlsoBeProxied()
        {
            //Arrange
            var generator = new ProxyGenerator();

            //Act
            var result = generator.Generate(typeof(ITestInterfaceExtended));

            //Assert
            Assert.IsTrue(result.Code.Contains("public System.Boolean Baz()"));
            Assert.IsTrue(result.Code.Contains("public System.Int32 get_Bar()"));
            Assert.IsTrue(result.Code.Contains("public void Foo()"));
        }

        [TestMethod]
        public void ProxyImplementsIDynamicProxy()
        {
            //Arrange
            var generator = new ProxyGenerator();

            //Act
            var result = generator.Generate(typeof(ITestInterfaceExtended));

            //Assert
            Assert.IsTrue(result.Code.Contains("AdvisorsConfiguration IHaveConfiguration.Configuration"));
        }
    }
}
