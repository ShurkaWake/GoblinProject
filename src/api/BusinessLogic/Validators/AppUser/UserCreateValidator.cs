using BusinessLogic.ViewModels.AppUser;
using FluentValidation;

namespace BusinessLogic.Validators.AppUser
{
    public class UserCreateValidator : AbstractValidator<UserCreateModel>
    {
        public UserCreateValidator() 
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.FullName).NotEmpty();
        }
    }
}
