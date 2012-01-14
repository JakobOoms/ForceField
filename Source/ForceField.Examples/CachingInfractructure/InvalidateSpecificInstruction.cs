using System.Reflection;

namespace ForceField.Examples.CachingInfractructure
{
    internal class InvalidateSpecificInstruction : IInvalidateInstruction
    {
        private readonly MemberInfo _memberInfo;

        public InvalidateSpecificInstruction(MemberInfo memberInfo)
        {
            _memberInfo = memberInfo;
        }

        public bool InvalidateOn(MemberInfo memberInfo)
        {
            return Equals(_memberInfo, memberInfo);
        }
    }
}