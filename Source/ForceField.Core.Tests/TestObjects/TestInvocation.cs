using System.Reflection;
using ForceField.Core.Invocation;

namespace ForceField.Core.Tests.TestObjects
{
    internal class TestInvocation : IInvocation
    {
        public MethodInfo MethodInfo { get; private set; }
        public void Proceed(){}
        public IIndexerEnumerable<InvocationArgument, string> Arguments { get; private set; }
        public object ReturnValue { get; set; }
        public object Target { get; private set; }
    }
}