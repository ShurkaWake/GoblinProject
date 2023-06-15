using BusinessLogic.ViewModels.Business;
using FluentValidation;

namespace BusinessLogic.Validators.Business
{
    public class BusinessCreateValidator : AbstractValidator<BusinessCreateModel>
    {
        public BusinessCreateValidator() 
        {
            RuleFor(x => x.Name).NotEmpty().MinimumLength(6);
            RuleFor(x => x.Location).NotEmpty();
        }
    }
}
