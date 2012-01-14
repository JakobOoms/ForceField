using System;
using ForceField.Core.Invocation;
using ForceField.Core.Pointcuts;

namespace ForceField.Examples.Advices
{
    public class ApplyAdviceOnAllRepositories : IPointcut
    {
        public bool IsApplicableFor(Type type)
        {
            return type.Name.EndsWith("Repository");
        }

        public bool IsApplicableFor(IInvocation invocation)
        {
            return true;
        }
    }
}
