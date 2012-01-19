using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ForceField.Core.Tests
{
    [TestClass]
    public class GuardTest
    {
        #region NotNull
        [TestMethod]
        public void GuardNotNullWithInstanceShouldNotThrowException()
        {
            InvokeGuardIsNotNullMethod(this);
        }

        [TestMethod]
        public void GuardNotNullTestWithNullShouldTrowException()
        {
            Expect.ArgumentNullException(() => InvokeGuardIsNotNullMethod(null), e => e.ParamName == "guardTest");
        }

        private void InvokeGuardIsNotNullMethod(GuardTest guardTest)
        {
            Guard.ArgumentIsNotNull(() => guardTest);
        }

        #endregion

        #region NotEqual

        [TestMethod]
        public void GuardNotEqualShouldNotThrowWhenObjectsAreNotEqual()
        {
            InvokeGuardNotEqualsMethod(42);
        }

        [TestMethod]
        public void GuardNotEqualShouldThrowWhenObjectsAreEqual()
        {
            Expect.ArgumentOutOfRangeException(() => InvokeGuardNotEqualsMethod(3), e => e.ParamName == "blockedValue");
        }

        private void InvokeGuardNotEqualsMethod(int blockedValue)
        {
            Guard.ArgumentIsNotEqualTo(() => blockedValue, 3);
        }

        #endregion
    }
}
