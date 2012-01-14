using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace ForceField.Core.Invocation
{
    public class FunctionInvocation<T, TReturnType> : BaseInvocation, IInvocation<T, TReturnType>
    {
        private readonly Func<T, TReturnType> _proceedMethod;

        [DebuggerStepThrough]
        public FunctionInvocation(MethodInfo methodInfo, IEnumerable<InvocationArgument> arguments, T proceedingTarget, Func<T, TReturnType> proceedingMethod)
            : base(methodInfo, arguments)
        {
            _proceedMethod = proceedingMethod;
            Target = proceedingTarget;
        }

        public override void Proceed()
        {
            ReturnValue = _proceedMethod(Target);
        }

        public TReturnType ReturnValue { get; set; }

        public T Target { get; private set; }

        object IInvocation.ReturnValue
        {
            get { return ReturnValue; }
            set { ReturnValue = (TReturnType)value; }
        }

        object IInvocation.Target
        {
            get { return Target; }
        }
    }
}