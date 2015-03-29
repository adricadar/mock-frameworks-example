using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Application.Mock.BL.Services;
using Application.Mock.BL.Validators;
using Application.Mock.DAL.Entities;
using Application.Mock.DAL.Repositories;
using Moq.AutoMock;
using NUnit.Framework;
using Moq;

namespace Application.Mock.BL.MoqTests
{
    [TestFixture]
    public class PersonServiceTests
    {
        #region Initial
        [Test]
        public void RegisterNewPerson_SaveThePerson_WhenThePersonIsValid()
        {
            //Arrange
            var mockPersonRepository = new Mock<IPersonRepository>();
            var personValidator = new Mock<IValidator<Person>>();
            personValidator.Setup(x => x.Validate(It.IsAny<Person>())).Returns(true);
            var personService = new PersonService(mockPersonRepository.Object, personValidator.Object);

            var person = new Person
            {
                Id = 123,
                FirstName = "Adrian",
                LastName = "Cadar"
            };

            //Act
            personService.Register(person);

            //Assert
            mockPersonRepository.Verify(m => m.Save(person), Times.Once);
        }

        [Test]
        public void RegisterNewPerson_ThrowsException_WhenThePersonIsNotValid()
        {
            //Arrange
            var mockPersonRepository = new Mock<IPersonRepository>();
            var personValidator = new Mock<IValidator<Person>>();
            var personService = new PersonService(mockPersonRepository.Object, personValidator.Object);
            try
            {
                personService.Register(null);
                Assert.Fail("ArgumentException excepted");
            }
            catch (Exception e)
            {
                Assert.IsInstanceOf<ArgumentException>(e);
            }
        }
        #endregion

        #region Argument Constraints
        [Test]
        public void ParamsRegisterNewPerson_SaveThePerson_WhenThePersonIsValid()
        {
            //Arrange
            var mockPersonRepository = new Mock<IPersonRepository>();
            var personValidator = new Mock<IValidator<Person>>();
            personValidator.Setup(x => x.Validate(It.IsAny<Person>())).Returns(true);
            var personService = new PersonService(mockPersonRepository.Object, personValidator.Object);


            int id = 123;
            string firstName = "Adrian";
            string lastName = "Cadar";

            //Act
            personService.Register(id, firstName, lastName);

            //Assert
            mockPersonRepository.Verify(x => x.Save(It.Is<Person>(p => p.Id == id && p.FirstName == firstName && p.LastName == lastName)));

        }

        [Test]
        public void DeletePersons_DeletesAllPersonsPassedIn()
        {
            //Arrange
            const int personId1 = 123;
            const int personId2 = 343;
            const int personId3 = 654;

            Person person1 = new Person { Id = personId1, FirstName = "Adrian", LastName = "Cadar" };
            Person person2 = new Person { Id = personId2, FirstName = "Creaza", LastName = "Persoane" };
            Person person3 = new Person { Id = personId3, FirstName = "Catmai", LastName = "Fakes" };

            var mockPersonRepository = new Mock<IPersonRepository>();
            mockPersonRepository.Setup(x => x.FindById(personId1)).Returns(person1);
            mockPersonRepository.Setup(x => x.FindById(personId2)).Returns(person2);
            mockPersonRepository.Setup(x => x.FindById(personId3)).Returns(person3);

            var personService = new PersonService(mockPersonRepository.Object, null);

            // Act
            personService.DeletePersons(personId1, personId2, personId3);

            //Assert
            mockPersonRepository.Verify(m => m.DeletePersons(It.Is<List<Person>>(l => l.Count == 3)));
            mockPersonRepository.Verify(m => m.DeletePersons(It.Is<List<Person>>(l => l.Contains(person1))));
        }
        /**/
        [Test]
        public void GetPersonByFirstName_GetThePersonFromRepository()
        {
            // Arrange
            var mockPersonRepository = new Mock<IPersonRepository>();
            var personService = new PersonService(mockPersonRepository.Object, null);

            // Act
            personService.GetListOfProducts("Jones");

            // Assert
            mockPersonRepository.Verify(x => x.GetPersonByFirstName(It.Is<string>(s => s.Contains("one"))));
            mockPersonRepository.Verify(x => x.GetPersonByFirstName(It.Is<string>(s => s.EndsWith("nes"))));
            mockPersonRepository.Verify(x => x.GetPersonByFirstName(It.Is<string>(s => s.StartsWith("Jon"))));
            //mockPersonRepository.Verify(x => x.GetPersonByFirstName(It.Is<string>(s => Like(s,"Jone.*"))));
        }

        #endregion

        #region Out/Ref Parameters
        [Test]
        public void ParamsRegisterNewPerson_SaveThePerson_WhenThePersonIsValid_WithOutRef()
        {
            //Arrange
            var mockPersonRepository = new Mock<IPersonRepository>();
            var personValidator = new Mock<IValidator<Person>>();
            var personService = new PersonService(mockPersonRepository.Object, personValidator.Object);

            int id = 123;
            string firstName = "Adrian";
            string lastName = "Cadar";

            Person person = new Person { Id = id, FirstName = firstName, LastName = lastName };
            // Out
            personValidator.Setup(x => x.ValidateOut(It.IsAny<Person>(), out person)).Returns(true);

            // Only matches if the ref argument to the invocation is the same instance
            personValidator.Setup(x => x.ValidateRef(It.IsAny<Person>(), ref person)).Returns(true); // <- must see

            //Act
            personService.RegisterOut(id, firstName, lastName);
            personService.RegisterRef(id, firstName, lastName);

            //Assert
            mockPersonRepository.Verify(x => x.Save(It.Is<Person>(p => p.Id == person.Id && p.FirstName == person.FirstName && p.LastName == person.LastName)));
        }
        #endregion
    }
}
