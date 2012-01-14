using System;
using ForceField.Core.Invocation;

namespace ForceField.Core.Pointcuts
{
    public class AlwaysApplyAdvice : IPointcut
    {
        public bool IsApplicableFor(Type type)
        {
            return true;
        }

        public bool IsApplicableFor(IInvocation invocation)
        {
            return true;
        }
    }
}
