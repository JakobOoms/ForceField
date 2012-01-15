using ForceField.Core.Advices;
using ForceField.Core.Invocation;

namespace ForceField.Core.Tests.TestObjects
{
    public class TestAdvice : IAdvice
    {
        public void ApplyAdvice(IInvocation invocation)
        {
            invocation.Proceed();
        }
    }
}