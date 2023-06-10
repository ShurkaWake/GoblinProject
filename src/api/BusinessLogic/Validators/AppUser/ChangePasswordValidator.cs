using BusinessLogic.ViewModels.AppUser;
using FluentValidation;
using static BusinessLogic.Validators.AppUser.Rules;

namespace BusinessLogic.Validators.AppUser
{
    public class ChangePasswordValidator : AbstractValidator<UserChangePasswordModel>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.OldPassword).Matches(PasswordRegex).WithMessage(PasswordMessage);
            RuleFor(x => x.NewPassword).Matches(PasswordRegex).WithMessage(PasswordMessage);
        }
    }
}
