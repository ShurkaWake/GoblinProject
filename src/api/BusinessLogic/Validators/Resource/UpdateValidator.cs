using BusinessLogic.Validators.MoneyAmount;
using BusinessLogic.ViewModels.Resource;
using FluentValidation;

namespace BusinessLogic.Validators.Resource
{
    public class UpdateValidator : AbstractValidator<ResourceUpdateModel>
    {
        public UpdateValidator() 
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Ammortization).SetValidator(new MoneyValidator());
        }
    }
}
