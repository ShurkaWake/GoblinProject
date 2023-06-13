using BusinessLogic.ViewModels.Business;
using FluentValidation;

namespace BusinessLogic.Validators.Business
{
    public class BusinessUpdateValidator : AbstractValidator<BusinessUpdateModel>
    {
        public BusinessUpdateValidator() 
        {
            RuleFor(x => x.Name).NotEmpty().MinimumLength(6);
            RuleFor(x => x.Location).NotEmpty();
        }
    }
}
