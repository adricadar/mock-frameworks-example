using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Mock.DAL.Entities;

namespace Application.Mock.BL.Services
{
    public interface IPersonService
    {
        void Register(Person person);
        void Register(int id, string firstName, string lastName);
        void DeletePersons(params int[] personIds);
        void Delete(Person person);
        Person GetListOfProducts(string firstName);
    }
}
