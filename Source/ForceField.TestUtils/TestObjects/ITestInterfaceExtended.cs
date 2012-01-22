namespace ForceField.TestUtils.TestObjects
{
    public interface ITestInterfaceExtended : ITestInterface
    {
        int Bar { get; }
        bool Baz();
    }
}