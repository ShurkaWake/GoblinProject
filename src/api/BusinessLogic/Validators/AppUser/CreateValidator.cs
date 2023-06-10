using BusinessLogic.ViewModels.AppUser;
using FluentValidation;

namespace BusinessLogic.Validators.AppUser
{
    public class CreateValidator : AbstractValidator<UserCreateModel>
    {
        public CreateValidator() 
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.FullName).NotEmpty();
        }
    }
}
