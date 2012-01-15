using ForceField.Core.Advices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ForceField.Core.Invocation;
using Moq;

namespace ForceField.Core.Tests
{
    [TestClass]
    public class EmptyAdviceTest
    {
        [TestMethod]
        public void ApplyAdviceTest()
        {
            //Arrange
            var target = new EmptyAdvice();
            var mockedInvocation = new Mock<IInvocation>();

            //Act
            target.ApplyAdvice(mockedInvocation.Object);

            //Assert
            mockedInvocation.Verify(x => x.Proceed(), Times.Once());
        }
    }
}
