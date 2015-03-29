using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Mock.BL.Validators;
using Application.Mock.DAL.Entities;
using Application.Mock.DAL.Repositories;

namespace Application.Mock.BL.Services
{
    public class PersonService : IPersonService
    {
        public PersonService(IPersonRepository personRepository, IValidator<Person> personValidator)
        {
            PersonValidator = personValidator;
            PersonRepository = personRepository;
        }

        public IPersonRepository PersonRepository { get; private set; }
        public IValidator<Person> PersonValidator { get; private set; }

        public void Register(Person person)
        {
            var isValid = PersonValidator.Validate(person);
            if (isValid)
            {
                PersonRepository.Save(person);
            }
            else
            {
                throw new ArgumentException("Invalid person","person");
            }
        }


        public void Register(int id, string firstName, string lastName)
        {
            var person = new Person {Id = id, FirstName = firstName, LastName = lastName};

            var isValid = PersonValidator.Validate(person);
            if (isValid)
            {
                PersonRepository.Save(person);
            }
            else
            {
                throw new ArgumentException("Invalid person", "person");
            }
        }

        public void RegisterOut(int id, string firstName, string lastName)
        {
            var person = new Person { Id = id, FirstName = firstName, LastName = lastName };

            Person outPerson = null;
            var isValid =PersonValidator.ValidateOut(person, out outPerson);      
            if (isValid)
            {
                PersonRepository.Save(person);
            }
            else
            {
                throw new ArgumentException("Invalid person", "person");
            }
        }

        public void RegisterRef(int id, string firstName, string lastName)
        {
            var person = new Person { Id = id, FirstName = firstName, LastName = lastName };

            Person refPerson = null;
            var isValid = PersonValidator.ValidateRef(person, ref refPerson);
            if (isValid)
            {
                PersonRepository.Save(person);
            }
            else
            {
                throw new ArgumentException("Invalid person", "person");
            }
        }

        public void DeletePersons(params int[] personIds)
        {
            List<Person> persons = new List<Person>();
            foreach (var personId in personIds)
            {
                persons.Add(PersonRepository.FindById(personId));
            }
            PersonRepository.DeletePersons(persons);
        }

        public void Delete(Person person)
        {
            PersonRepository.Delete(person);
        }

        public Person GetListOfProducts(string firstName)
        {
            // mult cod
            return PersonRepository.GetPersonByFirstName(firstName);
        }


    }
}
