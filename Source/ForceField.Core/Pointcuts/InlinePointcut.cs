using System;
using ForceField.Core.Invocation;

namespace ForceField.Core.Pointcuts
{
    //TODO: internal or public? Might be interesting for small/easy pointcuts
    internal class InlinePointcut : IPointcut
    {
        private readonly Func<Type, bool> _interestedInType;
        private readonly Func<IInvocation, bool> _interestedInInvocation;

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
