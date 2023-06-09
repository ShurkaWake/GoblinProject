using BusinessLogic.ViewModels.AppUser;
using FluentValidation;
using static BusinessLogic.Validators.AppUser.Rules;

namespace BusinessLogic.Validators.AppUser
{
    public class LoginValidator : AbstractValidator<UserLoginModel>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).Matches(PasswordRegex).WithMessage(PasswordMessage);
        }
    }
}
