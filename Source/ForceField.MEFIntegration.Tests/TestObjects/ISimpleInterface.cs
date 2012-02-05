namespace ForceField.MEFIntegration.Tests.TestObjects
{
    public interface ISimpleInterface<out T>
    {
        T Foo();
    }
}