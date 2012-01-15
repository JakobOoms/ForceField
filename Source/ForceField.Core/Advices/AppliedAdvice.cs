using System;
using ForceField.Core.Invocation;
using ForceField.Core.Pointcuts;

namespace ForceField.Core.Advices
{
    public class AppliedAdvice
    {
        private readonly IPointcut _pointcut;

        public AppliedAdvice(IAdvice advice, IPointcut pointcut)
        {
            Guard.ArgumentNotNull(() => advice, () => pointcut);

            Advice = advice;
            _pointcut = pointcut;
        }

        public IAdvice Advice { get; private set; }

        public bool IsApplicableFor(Type type)
        {
            return _pointcut.IsApplicableFor(type);
        }

        public bool IsApplicableFor(IInvocation invocation)
        {
            return _pointcut.IsApplicableFor(invocation);
        }
    }
}