using System;
using ForceField.Core.Invocation;
using ForceField.Core.Pointcuts;

namespace ForceField.Core.Advices
{
    public static class ApplyAdvice
    {
        public static IPointcut OnEveryMethod
        {
            get { return new InlinePointcut(x => true, x => true); }
        }

        public static IPointcut On(Func<Type, bool> adviceOnType)
        {
            return new InlinePointcut(adviceOnType, x => true);
        }

        public static IPointcut On(Func<Type, bool> adviceOnType, Func<IInvocation, bool> adviceOnInvocation)
        {
            return new InlinePointcut(adviceOnType, adviceOnInvocation);
        }
    }
}
