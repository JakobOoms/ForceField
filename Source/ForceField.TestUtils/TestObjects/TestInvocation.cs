using System.Reflection;
using ForceField.Core;
using ForceField.Core.Invocation;

namespace ForceField.TestUtils.TestObjects
{
    public class TestInvocation : IInvocation
    {
        public MethodInfo MethodInfo { get; private set; }
        public void Proceed(){}
        public IIndexerEnumerable<InvocationArgument, string> Arguments { get; private set; }
        public object ReturnValue { get; set; }
        public object Target { get; private set; }
    }
}