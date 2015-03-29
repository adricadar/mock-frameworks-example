using System.Collections.Generic;
using Application.Mock.DAL.Entities;

namespace Application.Mock.DAL.Repositories
{
    public interface IPersonRepository
    {
        Person FindById(int person);
        void Save(Person person);
        void DeletePersons(List<Person> persons);
        void Delete(Person person);
        Person GetPersonByFirstName(string firstName);
    }
}
