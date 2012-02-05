using System.ComponentModel.Composition;

namespace ForceField.MEFIntegration.Tests.TestObjects
{
    [Export(typeof(ITargetWithConstructorDepedency))]
    public class TargetWithConstructorDepedency : ITargetWithConstructorDepedency
    {
        public ISimpleInterface<int> Dependency { get; set; }

        [ImportingConstructor]
        public TargetWithConstructorDepedency([Import]ISimpleInterface<int> simpleInterface )
        {
            Dependency = simpleInterface;
        }

        public void ConsumeDependency()
        {
            Dependency.Foo();
        }
    }
}