using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace ForceField.Core.Invocation
{
    public class VoidInvocation<T> : BaseInvocation, IInvocation<T>
    {
        private readonly Action<T> _proceedMethod;

        [DebuggerStepThrough]
        public VoidInvocation(MethodInfo methodInfo, IEnumerable<InvocationArgument> arguments, T proceedingTarget, Action<T> proceedingMethod)
            : base(methodInfo, arguments)
        {
            Target = proceedingTarget;
            _proceedMethod = proceedingMethod;
        }

        public override void Proceed()
        {
            _proceedMethod(Target);
        }

        object IInvocation.ReturnValue
        {
            get { return null; }
            set { throw new TargetInvocationException("Cannot set returnvalue of method that returns void", null); }
        }

        object IInvocation.Target
        {
            get { return Target; }
        }

        public T Target { get; private set; }
    }
}