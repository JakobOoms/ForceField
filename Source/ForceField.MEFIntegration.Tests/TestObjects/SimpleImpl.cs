using System.ComponentModel.Composition;

namespace ForceField.MEFIntegration.Tests.TestObjects
{
    [Export(typeof(ISimpleInterface<int>))]
    public class SimpleImpl : ISimpleInterface<int>
    {
        public int Foo()
        {
            return 42;
        }
    }
}