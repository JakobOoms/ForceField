namespace ForceField.Core.Generator
{
    /// <summary>
    /// Container that is used to pass the ProxyInstantiator from the dynamicly generated code from Roslyn back to 'our side'
    /// </summary>
    public class HostingContainer
    {
        public ProxyInstantiator ProxyInstantiator { get; set; }
    }
}