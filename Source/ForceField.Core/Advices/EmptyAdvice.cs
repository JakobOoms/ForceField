using ForceField.Core.Invocation;

namespace ForceField.Core.Advices
{
    //TODO: Is NullAdvice a better name?

    /// <summary>
    /// An empty advice, an advice which has no behaviour.
    /// </summary>
    public class EmptyAdvice : IAdvice
    {
        public void ApplyAdvice(IInvocation invocation)
        {
            invocation.Proceed();
        }
    }
}
