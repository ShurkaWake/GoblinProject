using BusinessLogic.ViewModels.Business;
using FluentValidation;

namespace BusinessLogic.Validators.Business
{
    public class CreateValidator : AbstractValidator<BusinessCreateModel>
    {
        public CreateValidator() 
        {
            RuleFor(x => x.Name).NotEmpty().MinimumLength(6);
            RuleFor(x => x.Location).NotEmpty();
        }
    }
}
