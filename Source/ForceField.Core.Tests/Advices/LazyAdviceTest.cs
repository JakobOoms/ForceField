using System;
using ForceField.Core.Advices;
using ForceField.Core.Tests.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ForceField.Core.Tests.Advices
{
    [TestClass]
    public class LazyAdviceTest
    {
        [TestMethod]
        public void CreateAndCallProceedOfInnerAdvice()
        {
            //Arrange
            var testInvocation = new TestInvocation();
            var mockedAdvice = new Mock<IAdvice>();
            var sut = new LazyAdvice<IAdvice>(() => mockedAdvice.Object);

            //Act
            sut.ApplyAdvice(testInvocation);

            //Assert
            mockedAdvice.Verify(x => x.ApplyAdvice(testInvocation), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(CannotInstantiateAdviceException))]
        public void ThrowExceptionIfInnerAdviceCannotBeBuild()
        {
            //Arrange
            var testInvocation = new TestInvocation();
            var sut = new LazyAdvice<TestAdviceWithoutParameterlessConstructor>(() => null);

            //Act
            sut.ApplyAdvice(testInvocation);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateMethodIsRequired()
        {
            new LazyAdvice<TestAdviceWithoutParameterlessConstructor>(null);
        }

        private class TestAdviceWithoutParameterlessConstructor : IAdvice
        {
            public TestAdviceWithoutParameterlessConstructor(bool foo)
            {
            }

            public void ApplyAdvice(Invocation.IInvocation invocation)
            {
                invocation.Proceed();
            }
        }
    }
}
