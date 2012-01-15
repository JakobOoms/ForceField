using ForceField.Core.Invocation;

namespace ForceField.Core.Advices
{
    public interface IAdvice
    {
        /// <summary>
        /// Applies the advice (the cross cutting concern) to a given invocation.
        /// It creates a forcefield around the cross cutting concern, so it doesn't leak into other services.
        /// </summary>
        /// <param name="invocation"></param>
        void ApplyAdvice(IInvocation invocation);
    }
}