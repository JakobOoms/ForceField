using ForceField.Core.Invocation;

namespace ForceField.Core.Advices
{
    public class EmptyAdvice : IAdvice
    {
        public void ApplyAdvice(IInvocation invocation)
        {
            invocation.Proceed();
        }
    }
}
