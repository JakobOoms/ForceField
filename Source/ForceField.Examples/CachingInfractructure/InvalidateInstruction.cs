using System;
using System.Reflection;

namespace ForceField.Examples.CachingInfractructure
{
    internal class InvalidateInstruction : IInvalidateInstruction
    {
        private readonly Func<MemberInfo, bool> _invalidateCriteria;

        public InvalidateInstruction(Func<MemberInfo, bool> invalidateCriteria)
        {
            _invalidateCriteria = invalidateCriteria;
        }

        public bool InvalidateOn(MemberInfo memberInfo)
        {
            return _invalidateCriteria(memberInfo);
        }
    }
}