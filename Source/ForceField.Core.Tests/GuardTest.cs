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
        [TestMethod]
        public void GuardNotNullWithInstanceShouldNotThrowException()
        {
            InvokeTest(this);
        }

        [TestMethod]
        public void GuardNotNullTestWithNullShouldTrowException()
        {
            bool correctException = false;
            try
            {
                InvokeTest(null);
            }
            catch (ArgumentNullException e)
            {
                correctException = (e.ParamName == "guardTest");
            }
            Assert.IsTrue(correctException, "Wrong or no exception thrown");
        }

        private void InvokeTest(GuardTest guardTest)
        {
            Guard.ArgumentNotNull(() => guardTest);
        }
    }
}
