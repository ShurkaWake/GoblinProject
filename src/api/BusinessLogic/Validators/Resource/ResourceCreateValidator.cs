using BusinessLogic.Validators.MoneyAmount;
using BusinessLogic.ViewModels.Resource;
using FluentValidation;

namespace BusinessLogic.Validators.Resource
{
    public class ResourceCreateValidator : AbstractValidator<ResourceCreateModel>
    {
        public ResourceCreateValidator() 
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Ammortization).SetValidator(new MoneyValidator());
        }
    }
}
