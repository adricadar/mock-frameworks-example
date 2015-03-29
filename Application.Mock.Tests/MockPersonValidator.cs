using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Mock.BL.Validators;
using Application.Mock.DAL.Entities;

namespace Application.Mock.Tests
{
    public class MockPersonValidator : IValidator<Person>
    {
        #region Implementation of IValidator<Person>

        public bool Validate(Person model)
        {
            return false;
        }

        public bool ValidateOut(Person model, out Person outModel)
        {
            outModel = null;
            return true;
        }

        public bool ValidateRef(Person model, ref Person outModel)
        {
            return true;
        }

        #endregion
    }
}
