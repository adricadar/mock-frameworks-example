using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Mock.BL.Services;
using Application.Mock.DAL.Entities;
using NUnit.Framework;

namespace Application.Mock.Tests
{
    [TestFixture]
    public class PersonServiceTests
    {
        [Test]
        public void RegisterNewPerson_SaveThePerson_WhenThePersonIsValid()
        {
            //Arrange
            var person = new Person
            {
                Id = 123,
                FirstName = "Adrian",
                LastName = "Cadar"
            };

            var mockPersonRepository = new MockPersonRepository();
            var mockPersonValidator = new MockPersonValidator();
            var personService = new PersonService(mockPersonRepository, mockPersonValidator);

            //Act
            personService.Register(person);

            //Assert
            Assert.IsTrue(mockPersonRepository.SaveWasCalled);
            Assert.AreEqual(mockPersonRepository.SaveWasCalledCounter, 1);
        }
    }
}
