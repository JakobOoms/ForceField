using ForceField.Core.Advices;
using ForceField.Core.Invocation;

namespace ForceField.TestUtils.TestObjects
{
    public class TestAdviceWithDependency : IAdvice
    {
        private readonly ITestInterfaceExtended _dependency;

        public TestAdviceWithDependency(ITestInterfaceExtended dependency)
        {
            _dependency = dependency;
        }

        public void ApplyAdvice(IInvocation invocation)
        {
            invocation.Proceed();
        }
    }
}