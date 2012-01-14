using System.Reflection;

namespace ForceField.Core.Invocation
{
    public interface IInvocation
    {
        MethodInfo MethodInfo { get; }
        void Proceed();
        IIndexerEnumerable<InvocationArgument, string> Arguments { get; }
        object ReturnValue { get; set; }
        object Target { get; }
    }

    public interface IInvocation<out TType> : IInvocation
    {
        new TType Target { get; }
    }

    public interface IInvocation<out TType, TReturnType> : IInvocation<TType>
    {
        new TReturnType ReturnValue { get; set; }
    }
}