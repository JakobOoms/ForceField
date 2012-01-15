namespace ForceField.Core.Tests.TestObjects
{
    internal class TestAdvisorConfiguration : AdvisorsConfiguration
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