using System;
using ForceField.Core.Invocation;

namespace ForceField.Core.Pointcuts
{
    public class InlinePointcut : IPointcut
    {
        private readonly Func<Type, bool> _interestedInType;
        private readonly Func<IInvocation, bool> _interestedInInvocation;


        public InlinePointcut(Func<Type, bool> interestedInType)
            : this(interestedInType, x => true)
        {
        }

        public InlinePointcut(Func<Type, bool> interestedInType, Func<IInvocation, bool> interestedInInvocation)
        {
            _interestedInType = interestedInType ?? (x => true);
            _interestedInInvocation = interestedInInvocation ?? (x => true);
        }

        public bool IsApplicableFor(Type type)
        {
            return _interestedInType(type);
        }

        public bool IsApplicableFor(IInvocation invocation)
        {
            return _interestedInInvocation(invocation);
        }
    }
}
