using System.Diagnostics;
using System.Reflection;

namespace ForceField.Core.Invocation
{
    [DebuggerDisplay("{Parameter.Name} =  {Value}")]
    public class InvocationArgument
    {
        [DebuggerStepThrough]
        public InvocationArgument(ParameterInfo parameter, object value)
        {
            Parameter = parameter;
            Value = value;
        }

        public ParameterInfo Parameter { get; private set; }
        public object Value { get; private set; }
    }
}