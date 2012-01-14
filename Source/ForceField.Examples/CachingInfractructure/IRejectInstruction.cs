using System.Reflection;

namespace ForceField.Examples.CachingInfractructure
{
    internal interface IRejectInstruction
    {
        bool Reject(MemberInfo member);
    }
}