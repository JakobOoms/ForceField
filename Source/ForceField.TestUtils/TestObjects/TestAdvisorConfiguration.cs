using ForceField.Core;

namespace ForceField.TestUtils.TestObjects
{
    public class TestAdvisorConfiguration : AdvisorsConfiguration
    {
        protected override T TryResolveAdvice<T>()
        {
            return null;
        }

        protected override AdvisorsConfiguration Clone()
        {
            return new TestAdvisorConfiguration();
        }
    }
}