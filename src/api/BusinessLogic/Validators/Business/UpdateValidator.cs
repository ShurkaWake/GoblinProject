using BusinessLogic.ViewModels.Business;
using FluentValidation;

namespace BusinessLogic.Validators.Business
{
    public class UpdateValidator : AbstractValidator<BusinessUpdateModel>
    {
        public UpdateValidator() 
        {
            RuleFor(x => x.Name).NotEmpty().MinimumLength(6);
            RuleFor(x => x.Location).NotEmpty();
        }
    }
}
