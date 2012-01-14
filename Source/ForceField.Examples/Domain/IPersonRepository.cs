namespace ForceField.Examples.Domain
{
    public interface IPersonRepository : IRepository<Person>
    {
        Person GetByName(string name);
    }
}