using BusinessLogic.ViewModels.AppUser;
using FluentValidation;

namespace BusinessLogic.Validators.AppUser
{
    public class UserUpdateValidator : AbstractValidator<UserUpdateModel>
    {
        public UserUpdateValidator() 
        {
            RuleFor(x => x.FullName).NotEmpty();
        }
    }
}
