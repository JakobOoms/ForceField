namespace ForceField.TestUtils.TestObjects
{
    public class TestInterfaceExtended : ITestInterfaceExtended
    {
        public void Foo()
        {
        }

        public int Bar
        {
            get { return 42; }
        }

        public bool Baz()
        {
            return false;
        }
    }
}