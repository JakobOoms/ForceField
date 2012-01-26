using System;
using ForceField.Core.Advices;
using ForceField.Core.Invocation;
using ForceField.Core.Pointcuts;
using ForceField.TestUtils.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ForceField.Core.Tests
{
    [TestClass]
    public class ProxyFactoryTests
    {
        [TestMethod]
        public void TypeCannotBeNull()
        {
            Expect.ArgumentNullException(() => ProxyFactory.Create<TestClass>(null, new TestBaseConfiguration()));
        }

        [TestMethod]
        public void ConfigurationCannotBeNull()
        {
            //Arrange
            var instance = new TestClass();

            //Act + Assert
            Expect.ArgumentNullException(() => ProxyFactory.Create(instance, null));
        }

        [TestMethod]
        public void DontCreateProxyWhenNoAdvicesAvailable()
        {
            //Arrange
            var instance = new TestClass();
            var config = new TestBaseConfiguration();

            //Act
            var proxied = ProxyFactory.Create(instance, config);

            //Assert
            Assert.AreEqual(instance, proxied);
        }

        [TestMethod]
        public void DontCreateProxyWhenNoAdvicesCanBeApplied()
        {
            //Arrange
            var instance = new TestClass();
            var config = new TestBaseConfiguration();
            var advice = new TestAdvice();
            config.Add(advice, new NeverApplyAdvice());

            //Act
            var proxied = ProxyFactory.Create(instance, config);

            //Assert
            Assert.AreEqual(instance, proxied);
        }

        [TestMethod]
        public void AdviceShouldBeCalledWhenPointcutSaysSo()
        {
            //Arrange
            var config = new TestBaseConfiguration();
            var mockedAdvice = new Mock<IAdvice>();
            config.Add(mockedAdvice.Object, new AlwaysApplyAdvice());
            var proxied = ProxyFactory.Create<ITestInterface>(new TestClass(), config);

            //Act
            proxied.Foo();

            //Assert
            mockedAdvice.Verify(x => x.ApplyAdvice(It.IsAny<IInvocation>()), Times.Once());
        }

    }
}
