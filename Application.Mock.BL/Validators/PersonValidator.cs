using Application.Mock.DAL.Entities;

namespace Application.Mock.BL.Validators
{
    public class PersonValidator : IValidator<Person>
    {
        public bool Validate(Person model)
        {
            return model != null;
        }

        public bool ValidateOut(Person model, out Person outModel)
        {
            throw new System.NotImplementedException();
        }

        public bool ValidateRef(Person model, ref Person outModel)
        {
            throw new System.NotImplementedException();
        }
    }
}