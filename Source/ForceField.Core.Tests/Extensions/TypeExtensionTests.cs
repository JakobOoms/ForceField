using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using ForceField.Core.Extensions;
using ForceField.Core.Invocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ForceField.Core.Tests.Extensions
{
    [TestClass]
    public class TypeExtensionTests
    {
        [TestMethod]
        public void IsVoidForVoidTypeIsTrueTest()
        {
            //Arrange
            var type = typeof(void);

            //Act
            var isVoid = type.IsVoid();

            //Assert
            Assert.IsTrue(isVoid);
        }

        [TestMethod]
        public void IsVoidForObjectTypeIsFalseTest()
        {
            //Arrange
            var type = typeof(object);

            //Act
            var isVoid = type.IsVoid();

            //Assert
            Assert.IsFalse(isVoid);
        }

        [TestMethod]
        public void IsVoidForFakeVoidTypeIsFalseTest()
        {
            //Arrange
            var type = typeof(Void);

            //Act
            var isVoid = type.IsVoid();

            //Assert
            Assert.IsFalse(isVoid);
        }
    }

    internal struct Void
    {
    }
}
