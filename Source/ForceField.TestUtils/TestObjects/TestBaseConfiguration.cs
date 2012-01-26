using ForceField.Core;

namespace ForceField.TestUtils.TestObjects
{
    public class TestBaseConfiguration : BaseConfiguration
    {
        protected override T TryResolveAdvice<T>()
        {
            return null;
        }

        protected override BaseConfiguration Clone()
        {
            return new TestBaseConfiguration();
        }
    }
}