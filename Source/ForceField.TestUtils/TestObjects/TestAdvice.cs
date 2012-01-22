using ForceField.Core.Advices;
using ForceField.Core.Invocation;

namespace ForceField.TestUtils.TestObjects
{
    public class TestAdvice : IAdvice
    {
        public void ApplyAdvice(IInvocation invocation)
        {
            invocation.Proceed();
        }
    }
}