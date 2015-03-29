using System;
using System.Collections.Generic;
using Application.Mock.BL.Services;
using Application.Mock.BL.Validators;
using Application.Mock.DAL.Entities;
using Application.Mock.DAL.Repositories;
using NUnit.Framework;
using Rhino.Mocks;
using Rhino.Mocks.Constraints;
using StructureMap.AutoMocking;

namespace Application.Mock.RhinoMocksTests
{
    [TestFixture]
    public class PersonServiceTests
    {
        #region Initial
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

            var mockPersonRepository = MockRepository.GenerateMock<IPersonRepository>();
            var mockPersonValidator = MockRepository.GenerateStub<IValidator<Person>>();
            mockPersonRepository.Expect(x => x.Save(person)).Repeat.Once();

            mockPersonValidator.Stub(x => x.Validate(Arg<Person>.Is.Anything)).Return(true);
            var personService = new PersonService(mockPersonRepository, mockPersonValidator );

            //Act
            personService.Register(person);

            //Assert
            //mockPersonRepository.AssertWasCalled(x => x.Save(person), x=>x.Repeat.Once());
            mockPersonRepository.VerifyAllExpectations();
        }


        [Test]
        public void RegisterNewPerson_ThrowsException_WhenThePersonIsNotValid()
        {
            var mockPersonRepository = MockRepository.GenerateMock<IPersonRepository>();
            var personValidator = MockRepository.GenerateMock<IValidator<Person>>();

            var personService = new PersonService(mockPersonRepository, personValidator);
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
            var mockPersonRepository = MockRepository.GenerateMock<IPersonRepository>();
            var personValidator = MockRepository.GenerateStub<IValidator<Person>>();
            personValidator.Stub(x => x.Validate(Arg<Person>.Is.Anything)).Return(true);
            var personService = new PersonService(mockPersonRepository, personValidator);


            int id = 123;
            string firstName = "Adrian";
            string lastName = "Cadar";

            //Act
            personService.Register(id, firstName, lastName);

            //Assert
            mockPersonRepository.AssertWasCalled(x => x.Save(Arg<Person>.Matches(p => p.Id == id && p.FirstName == firstName && p.LastName == lastName)));

        }

        [Test]
        public void DeletePersons_DeletesAllPersonsPassedIn()
        {
            //Arrange
            const int personId1 = 123;
            const int personId2 = 343;
            const int personId3 = 654;
            const int personId4 = 32;

            Person person1 = new Person { Id = personId1, FirstName = "Adrian", LastName = "Cadar" };
            Person person2 = new Person { Id = personId2, FirstName = "Creaza", LastName = "Persoane" };
            Person person3 = new Person { Id = personId3, FirstName = "Catmai", LastName = "Fakes" };
            Person person4 = new Person { Id = personId3, FirstName = "Catmai", LastName = "Fakes" };

            var mockPersonRepository = MockRepository.GenerateMock<IPersonRepository>();
            mockPersonRepository.Stub(x => x.FindById(personId1)).Return(person1);
            mockPersonRepository.Stub(x => x.FindById(personId2)).Return(person2);
            mockPersonRepository.Stub(x => x.FindById(personId3)).Return(person3);

            var personService = new PersonService(mockPersonRepository, null);

            // Act
            personService.DeletePersons(personId1, personId2, personId3);

            //Assert
            mockPersonRepository.AssertWasCalled(m => m.DeletePersons(Arg<List<Person>>.List.ContainsAll(new List<Person> { person1, person2, person3 })));
            mockPersonRepository.AssertWasCalled(m => m.DeletePersons(Arg<List<Person>>.List.Equal(new List<Person> { person1, person2, person3 })));
            mockPersonRepository.AssertWasCalled(m => m.DeletePersons(Arg<List<Person>>.List.Count(Rhino.Mocks.Constraints.Is.Equal(3))));
            mockPersonRepository.AssertWasCalled(m => m.DeletePersons(Arg<List<Person>>.List.IsIn(person1)));
        }

        [Test]
        public void GetPersonByFirstName_GetThePersonFromRepository()
        {
            // Arrange
            var mockPersonRepository = MockRepository.GenerateMock<IPersonRepository>();
            var personService = new PersonService(mockPersonRepository, null);

            // Act
            personService.GetListOfProducts("Jones");

            // Assert
            mockPersonRepository.AssertWasCalled(x => x.GetPersonByFirstName(Arg<string>.Matches(Rhino.Mocks.Constraints.Text.Contains("one"))));
            mockPersonRepository.AssertWasCalled(x => x.GetPersonByFirstName(Arg<string>.Matches(Rhino.Mocks.Constraints.Text.EndsWith("nes"))));
            mockPersonRepository.AssertWasCalled(x => x.GetPersonByFirstName(Arg<string>.Matches(Rhino.Mocks.Constraints.Text.StartsWith("Jon"))));
            mockPersonRepository.AssertWasCalled(x => x.GetPersonByFirstName(Arg<string>.Matches(Rhino.Mocks.Constraints.Text.Like("Jon.*"))));
        }

        #endregion

        #region Out/Ref Parameters
        [Test]
        public void ParamsRegisterNewPerson_SaveThePerson_WhenThePersonIsValid_WithOutRef()
        {
            //Arrange
            var mockPersonRepository = MockRepository.GenerateMock<IPersonRepository>();
            var personValidator = MockRepository.GenerateStub<IValidator<Person>>();
            var personService = new PersonService(mockPersonRepository, personValidator);

            int id = 123;
            string firstName = "Adrian";
            string lastName = "Cadar";

            Person person = new Person { Id = id, FirstName = firstName, LastName = lastName };
            // Out
            personValidator.Stub(x => x.ValidateOut(Arg<Person>.Is.Anything, out Arg<Person>.Out(person).Dummy)).Return(true);

            // Ref
            personValidator.Stub(x => x.ValidateRef(Arg<Person>.Is.Anything, ref Arg<Person>.Ref(Rhino.Mocks.Constraints.Is.Anything(), new Person()).Dummy)).Return(true);

            //Act
            personService.RegisterOut(id, firstName, lastName);
            personService.RegisterRef(id, firstName, lastName);
            //Assert
            mockPersonRepository.AssertWasCalled(x => x.Save(Arg<Person>.Matches(p => p.Id == person.Id && p.FirstName == person.FirstName && p.LastName == person.LastName)));

        }
        #endregion

        #region Record Replay
        [Test]
        public void Record_Replay_RegisterNewPerson_SaveThePerson_WhenThePersonIsValid()
        {
            //Arrange
            var person = new Person
            {
                Id = 123,
                FirstName = "Adrian",
                LastName = "Cadar"
            };
            var mocks = new MockRepository();
            var mockPersonRepository = mocks.DynamicMock<IPersonRepository>();
            var mockPersonValidator = mocks.DynamicMock<IValidator<Person>>();
            // mockPersonRepository.Expect(x => x.Save(person)).Repeat.Once();
           
            using (mocks.Record())
            {
                mockPersonValidator.Stub(x => x.Validate(Arg<Person>.Is.Anything)).Return(true);
                mockPersonRepository.Expect(x => x.Save(person)).Repeat.Once();
            }

            //Act
            using (mocks.Playback())
            {
                var personService = new PersonService(mockPersonRepository, mockPersonValidator);
                personService.Register(person);
            }

            //Assert
            //mockPersonRepository.AssertWasCalled(x => x.Save(person), x => x.Repeat.Once());
            //mocks.VerifyAllExpectations();
        }

        #endregion
    }

}
