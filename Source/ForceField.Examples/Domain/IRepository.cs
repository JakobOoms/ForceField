namespace ForceField.Examples.Domain
{
    public interface IRepository<T> where T : BaseEntity
    {
        void Save(T entity);
    }
}