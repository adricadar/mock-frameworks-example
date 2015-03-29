using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Mock.DAL.Entities;

namespace Application.Mock.DAL.Repositories
{
    public  class PersonRepository : IPersonRepository
    {
        public Person FindById(int person)
        {
            throw new NotImplementedException();
        }

        public void Save(Person person)
        {
            
        }

        public void DeletePersons(List<Person> persons)
        {
            throw new NotImplementedException();
        }

        public void Delete(Person person)
        {
            throw new NotImplementedException();
        }

        public Person GetPersonByFirstName(string firstName)
        {
            throw new NotImplementedException();
        }
    }
}
