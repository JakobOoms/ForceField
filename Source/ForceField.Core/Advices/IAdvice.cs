using ForceField.Core.Invocation;

namespace ForceField.Core.Advices
{
    public interface IAdvice
    {
        /// <summary>
        /// Applies the advice (the cross cutting concern) to a given invocation
        /// </summary>
        /// <param name="invocation"></param>
        void ApplyAdvice(IInvocation invocation);
    }
}