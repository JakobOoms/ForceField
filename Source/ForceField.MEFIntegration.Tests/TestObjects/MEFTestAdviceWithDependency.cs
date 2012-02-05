using System.ComponentModel.Composition;
using ForceField.Core.Advices;
using ForceField.Core.Invocation;

namespace ForceField.MEFIntegration.Tests.TestObjects
{
    [Export(typeof(MEFTestAdviceWithDependency))]
    public class MEFTestAdviceWithDependency : IAdvice
    {
        private readonly ISimpleInterface<int> _dependency;

        [ImportingConstructor]
        public MEFTestAdviceWithDependency([Import]ISimpleInterface<int> dependency)
        {
            _dependency = dependency;
        }

        public void ApplyAdvice(IInvocation invocation)
        {
            _dependency.Foo();
            invocation.Proceed();
        }
    }
}