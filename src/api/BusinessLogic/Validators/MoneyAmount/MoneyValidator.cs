using DataAccess.Entities;
using DataAccess.Enums;
using FluentValidation;

namespace BusinessLogic.Validators.MoneyAmount
{
    public class MoneyValidator : AbstractValidator<DataAccess.Entities.MoneyAmount>
    {
        public MoneyValidator() 
        {
            RuleFor(x => x.Amount).GreaterThan(0).NotEmpty();
            RuleFor(x => x.Currency).NotEmpty().IsInEnum();
        }
    }
}
