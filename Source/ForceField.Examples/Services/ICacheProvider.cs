namespace ForceField.Examples.Services
{
    //TODO: remove out from interface...: return Tuple<bool,object> ?
    public interface ICacheProvider
    {
        void Store(string key, object item);
        void Invalidate(string key);
        bool TryGet(string key, out object valueInCache);
    }
}