namespace ForceField.Core
{
    /// <summary>
    /// Interface that all dynamicly generated proxies will explicitly implement.
    /// In this way, the configuration of the proxy can still be altered at runtime.
    /// </summary>
    public interface IDynamicProxy
    {
        AdvisorsConfiguration Configuration { get; }
    }
}