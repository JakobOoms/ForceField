using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace ForceField.Core.Invocation
{
    public class FunctionInvocation<T, TReturnType> : IInvocation<T, TReturnType>
    {
        private readonly Func<T, TReturnType> _proceedMethod;

        [DebuggerStepThrough]
        public FunctionInvocation(MethodInfo methodInfo, IEnumerable<InvocationArgument> arguments, T proceedingTarget, Func<T, TReturnType> proceedingMethod)
        {
            MethodInfo = methodInfo;
            Arguments = new IndexedEnumerable<InvocationArgument, string>(x => x.Parameter.Name, arguments);
            _proceedMethod = proceedingMethod;
            Target = proceedingTarget;
        }

        public MethodInfo MethodInfo { get; private set; }
        public IIndexerEnumerable<InvocationArgument, string> Arguments { get; private set; }
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

        public void Proceed()
        {
            ReturnValue = _proceedMethod(Target);
        }
    }
}