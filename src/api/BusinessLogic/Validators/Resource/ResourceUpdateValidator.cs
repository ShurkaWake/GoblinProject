using BusinessLogic.Validators.MoneyAmount;
using BusinessLogic.ViewModels.Resource;
using FluentValidation;

namespace BusinessLogic.Validators.Resource
{
    public class ResourceUpdateValidator : AbstractValidator<ResourceUpdateModel>
    {
        public ResourceUpdateValidator() 
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Ammortization).SetValidator(new MoneyValidator());
        }
    }
}
