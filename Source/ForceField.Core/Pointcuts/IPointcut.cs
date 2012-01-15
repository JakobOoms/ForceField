using System;
using ForceField.Core.Invocation;

namespace ForceField.Core.Pointcuts
{
    public interface IPointcut
    {
        /// <summary>
        /// Defines whether the applied aAdvice is interested in the type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool IsApplicableFor(Type type);
        /// <summary>
        /// Defines whether the applied advice is interested in this particular invocation.
        /// </summary>
        /// <param name="invocation"></param>
        /// <returns></returns>
        bool IsApplicableFor(IInvocation invocation);
    }
}