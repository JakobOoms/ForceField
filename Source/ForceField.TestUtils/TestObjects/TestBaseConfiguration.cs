using ForceField.Core;

namespace ForceField.TestUtils.TestObjects
{
    public class TestBaseConfiguration : BaseConfiguration
    {
        protected override T ResolveAdvice<T>()
        {
            return null;
        }

        protected override BaseConfiguration Clone()
        {
            return new TestBaseConfiguration();
        }
    }
}