namespace ForceField.Core.Tests.TestObjects
{
    public interface ITestInterfaceExtended : ITestInterface
    {
        int Bar { get; }
        bool Baz();
    }
}