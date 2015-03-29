using Application.Mock.DAL.Entities;

namespace Application.Mock.BL.Validators
{
    public interface IValidator<TModel>
    {
        bool Validate(TModel model);
        bool ValidateOut(TModel model, out TModel outModel);
        bool ValidateRef(Person model, ref Person outModel);
    }
}