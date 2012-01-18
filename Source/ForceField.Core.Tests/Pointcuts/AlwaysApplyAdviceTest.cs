using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using ForceField.Core.Pointcuts;
using ForceField.Core.Tests.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ForceField.Core.Tests.Pointcuts
{
    [TestClass]
    public class AlwaysApplyAdviceTest
    {
        [TestMethod]
        public void AllTypesMatchesTest()
        {
            //Arrange
            var sut = new AlwaysApplyAdvice();

            //Act
            var thisTypeMatches =  sut.IsApplicableFor(GetType());
            var voidMatches = sut.IsApplicableFor(typeof (void));

            //Assert
            Assert.IsTrue(thisTypeMatches);
            Assert.IsTrue(voidMatches);
        }

        [TestMethod]
        public void AllInvocationsMatchesTest()
        {
             //Arrange
            var sut = new AlwaysApplyAdvice();
            var invocation = new TestInvocation();

            //Act
            var invocationMatches =  sut.IsApplicableFor(invocation);

            //Assert
            Assert.IsTrue(invocationMatches);
        }
    }
}
