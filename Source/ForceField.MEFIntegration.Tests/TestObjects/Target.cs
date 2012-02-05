using System.ComponentModel.Composition;

namespace ForceField.MEFIntegration.Tests.TestObjects
{
    public class Target
    {
        [Import]
        public ISimpleInterface<int> Dependency { get; set; }

        public void Run()
        {
            Dependency.Foo();
        }
    }
}