using System;
using System.Reflection;

namespace ForceField.Examples.CachingInfractructure
{
    internal class AcceptInstruction : IAcceptInstruction
    {
        private readonly Func<MemberInfo, bool> _acceptCriteria;
        private readonly CacheInstruction _instruction;


        public AcceptInstruction(Func<MemberInfo, bool> acceptCriteria, CacheInstruction instruction)
        {
            _acceptCriteria = acceptCriteria;
            _instruction = instruction;
        }

        public bool Accept(MemberInfo member)
        {
            return _acceptCriteria(member);
        }

        public CacheInstruction Instruction
        {
            get { return _instruction; }
        }
    }
}