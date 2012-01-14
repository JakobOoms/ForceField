using System;
using System.Reflection;

namespace ForceField.Examples.CachingInfractructure
{
    internal class RejectInstruction : IRejectInstruction
    {
        private readonly Func<MemberInfo, bool> _rejectCriteria;

        public RejectInstruction(Func<MemberInfo, bool> rejectCriteria)
        {
            _rejectCriteria = rejectCriteria;
        }

        public bool Reject(MemberInfo member)
        {
            return _rejectCriteria(member);
        }
    }
}