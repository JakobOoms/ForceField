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
        public void ThrowExceptionIfInnerAdviceCannotBeBuild()
        {
            //Arrange
            var testInvocation = new TestInvocation();
            var sut = new LazyAdvice<TestAdviceWithoutParameterlessConstructor>(() => null);

            //Act + Assert
            Expect.WillThrow<CannotInstantiateAdviceException>(() => sut.ApplyAdvice(testInvocation), e => e.Message.Contains(typeof(TestAdviceWithoutParameterlessConstructor).Name));
        }

        [TestMethod]
        public void CreateMethodIsRequired()
        {
            Expect.ArgumentNullException(() => new LazyAdvice<TestAdviceWithoutParameterlessConstructor>(null));
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
