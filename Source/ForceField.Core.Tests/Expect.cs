using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ForceField.Core.Tests
{
    public static class Expect
    {
        public static void ArgumentNullException(Action action, params Func<ArgumentNullException, bool>[] expectations)
        {
            WillThrow(action, expectations);
        }

        public static void ArgumentOutOfRangeException(Action action, params Func<ArgumentOutOfRangeException, bool>[] expectations)
        {
            WillThrow(action, expectations);
        }

        public static void WillThrow<TException>(Action action, params Func<TException, bool>[] expectations) where TException : Exception
        {
            bool isCorrectExceptionThrown = false;
            try
            {
                action();
            }
            catch (Exception e)
            {
                Assert.IsTrue(typeof(TException) == e.GetType(), "Exception that was thrown was of a wrong type. " + e.GetType() + " instead of " + typeof(TException));
                var exception = (TException)e;
                foreach (var expectation in expectations)
                {
                    Assert.IsTrue(expectation(exception), "One or more of the expectations of the thrown " + exception.GetType().Name + " was not met");
                }
                isCorrectExceptionThrown = true;
            }
            if (!isCorrectExceptionThrown)
            {
                Assert.Fail("No exception thrown, expected a " + typeof(TException) + " to be thrown");
            }
        }
    }
}