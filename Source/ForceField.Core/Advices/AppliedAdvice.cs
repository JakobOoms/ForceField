using System;
using ForceField.Core.Invocation;
using ForceField.Core.Pointcuts;

namespace ForceField.Core.Advices
{
    public class AppliedAdvice
    {
        private readonly IPointcut _applicability;

        public AppliedAdvice(IAdvice advice, IPointcut applicability)
        {
            Advice = advice;
            _applicability = applicability;
        }

        public IAdvice Advice { get; private set; }

        public bool IsApplicableFor(Type type)
        {
            return _applicability.IsApplicableFor(type);
        }

        public bool IsApplicableFor(IInvocation invocation)
        {
            return _applicability.IsApplicableFor(invocation);
        }
    }
}