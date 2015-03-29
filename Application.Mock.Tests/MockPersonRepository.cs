using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Mock.DAL.Entities;
using Application.Mock.DAL.Repositories;

namespace Application.Mock.Tests
{
    public class MockPersonRepository : IPersonRepository
    {
        public bool SaveWasCalled { get; set; }
        public int SaveWasCalledCounter { get; set; }

        #region Implementation of IPersonRepository

        public Person FindById(int person)
        {
            return null;
        }

        public void Save(Person person)
        {
            SaveWasCalled = true;
            SaveWasCalledCounter++;
        }

        public void DeletePersons(List<Person> persons)
        {
            
        }

        public void Delete(Person person)
        {
            
        }

        public Person GetPersonByFirstName(string firstName)
        {
            return null;
        }

        #endregion
    }
}
