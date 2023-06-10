using BusinessLogic.ViewModels.AppUser;
using FluentValidation;

namespace BusinessLogic.Validators.AppUser
{
    public class UpdateValidator : AbstractValidator<UserUpdateModel>
    {
        public UpdateValidator() 
        {
            RuleFor(x => x.FullName).NotEmpty();
        }
    }
}
