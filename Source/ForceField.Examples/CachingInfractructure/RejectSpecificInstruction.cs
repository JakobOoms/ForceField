using System.Reflection;

namespace ForceField.Examples.CachingInfractructure
{
    internal class RejectSpecificInstruction : IRejectInstruction
    {
        private readonly MemberInfo _rejectedMember;

        public RejectSpecificInstruction(MemberInfo rejectedMember)
        {
            _rejectedMember = rejectedMember;
        }

        public bool Reject(MemberInfo member)
        {
            return Equals(member, _rejectedMember);
        }
    }
}