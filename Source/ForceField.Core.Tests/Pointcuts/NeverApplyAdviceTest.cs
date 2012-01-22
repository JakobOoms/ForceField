using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using ForceField.Core.Pointcuts;
using ForceField.TestUtils.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ForceField.Core.Tests.Pointcuts
{
    [TestClass]
    public class NeverApplyAdviceTest
    {
        [TestMethod]
        public void NoTypesMatchesTest()
        {
            //Arrange
            var sut = new NeverApplyAdvice();

            //Act
            var thisTypeMatches = sut.IsApplicableFor(GetType());
            var voidMatches = sut.IsApplicableFor(typeof(void));

            //Assert
            Assert.IsFalse(thisTypeMatches);
            Assert.IsFalse(voidMatches);
        }

        [TestMethod]
        public void NoInvocationsMatchesTest()
        {
            //Arrange
            var sut = new NeverApplyAdvice();
            var invocation = new TestInvocation();

            //Act
            var invocationMatches = sut.IsApplicableFor(invocation);

            //Assert
            Assert.IsFalse(invocationMatches);
        }
    }
}
