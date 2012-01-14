using System.Reflection;

namespace ForceField.Examples.CachingInfractructure
{
    internal interface IAcceptInstruction
    {
        bool Accept(MemberInfo member);
        CacheInstruction Instruction { get; }
    }
}