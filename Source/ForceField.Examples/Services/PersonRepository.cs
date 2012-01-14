using System.Collections.Generic;
using System.Linq;
using ForceField.Examples.Domain;

namespace ForceField.Examples.Services
{
    public class PersonRepository : IPersonRepository
    {
        private readonly ILoggingService _loggingService;
        private readonly List<Person> _persons = new List<Person>();

        public PersonRepository(ILoggingService loggingService)
        {
            _loggingService = loggingService;
            _persons.Add(new Person { Name = "John", Age = 26, Key = 1 });
        }

        public Person GetByName(string name)
        {
            return _persons.FirstOrDefault(x => x.Name == name);
        }

        public void Save(Person entity)
        {
            _persons.Remove(entity);
            _persons.Add(entity);
        }

        public Person Load(int key)
        {
            return _persons.FirstOrDefault(x => x.Key == key);
        }
    }
}