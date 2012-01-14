using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace ForceField.Core.Invocation
{
    //TODO: remove the BaseInvocation class, as it is not aligned with the interface IInvocation
    //or make it implement the interface....
    public abstract class BaseInvocation
    {
        [DebuggerStepThrough]
        protected BaseInvocation(MethodInfo methodInfo, IEnumerable<InvocationArgument> arguments)
        {
            MethodInfo = methodInfo;
            Arguments = new IndexedEnumerable<InvocationArgument, string>(x => x.Parameter.Name, arguments);
        }

        public MethodInfo MethodInfo { get; private set; }
        public abstract void Proceed();
        public IIndexerEnumerable<InvocationArgument, string> Arguments { get; private set; }
    }
}