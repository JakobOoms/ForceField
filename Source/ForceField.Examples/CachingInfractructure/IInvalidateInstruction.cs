using System.Reflection;

namespace ForceField.Examples.CachingInfractructure
{
    internal interface IInvalidateInstruction
    {
        bool InvalidateOn(MemberInfo memberInfo);
    }
}