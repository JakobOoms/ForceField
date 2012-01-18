﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ForceField.Core.Generator;
using ForceField.Core.Tests.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ForceField.Core.Tests.Generator
{
    [TestClass]
    public class ProxyGeneratorTest
    {
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
    }
}