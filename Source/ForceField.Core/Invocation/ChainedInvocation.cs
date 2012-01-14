using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using ForceField.Core.Advices;

namespace ForceField.Core.Invocation
{
    //TODO: should not be public, but is now directly referenced from the generated code.
    //Maybe the generated code should use a factory method that creates this ChainedInvocation and just returns this as an IInvocation 
    public class ChainedInvocation : IInvocation
    {
        private readonly IInvocation _innerInvocation;
        private readonly Queue<IAdvice> _nextAdvices;

        [DebuggerStepThrough]
        public ChainedInvocation(IInvocation innerInvocation, IEnumerable<IAdvice> nextAdvices)
        {
            _innerInvocation = innerInvocation;
            _nextAdvices = new Queue<IAdvice>(nextAdvices);
        }

        public MethodInfo MethodInfo
        {
            get { return _innerInvocation.MethodInfo; }
        }

        public void Proceed()
        {
            if (_nextAdvices.Count > 0)
            {
                //Go deeper down the chain: first execute all remaining advices for this invocation
                var nextAdvice = _nextAdvices.Dequeue();
                nextAdvice.ApplyAdvice(new ChainedInvocation(_innerInvocation, _nextAdvices));
            }
            else
            {
                //All advices are applied, so proceed with the real invocation on the proxied object
                _innerInvocation.Proceed();
            }
        }

        public IIndexerEnumerable<InvocationArgument, string> Arguments
        {
            get { return _innerInvocation.Arguments; }
        }

        public object ReturnValue
        {
            get { return _innerInvocation.ReturnValue; }
            set { _innerInvocation.ReturnValue = value; }
        }

        public object Target
        {
            get { return _innerInvocation.Target; }
        }
    }
}
