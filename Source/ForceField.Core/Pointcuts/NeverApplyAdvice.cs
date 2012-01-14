using System;
using ForceField.Core.Invocation;

namespace ForceField.Core.Pointcuts
{
    public class NeverApplyAdvice : IPointcut
    {
        public bool IsApplicableFor(Type type)
        {
            return false;
        }

        public bool IsApplicableFor(IInvocation invocation)
        {
            return false;
        }
    }
}
