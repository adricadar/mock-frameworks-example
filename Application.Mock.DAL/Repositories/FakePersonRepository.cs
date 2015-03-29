using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Mock.DAL.Entities;
using Moq;

namespace Application.Mock.DAL.Repositories
{
    public class FakePersonRepository : Mock<IPersonRepository>
    {
        public FakePersonRepository()
        {
            base.Setup(x => x.FindById(It.IsAny<int>())).Returns(new Person());

        }
    }
}
