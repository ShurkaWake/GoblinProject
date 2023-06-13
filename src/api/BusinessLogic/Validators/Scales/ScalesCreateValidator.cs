using BusinessLogic.ViewModels.Scales;
using FluentValidation;

namespace BusinessLogic.Validators.Scales
{
    public class ScalesCreateValidator : AbstractValidator<ScalesCreateModel>
    {
        public ScalesCreateValidator() 
        {
            RuleFor(x => x.SerialNumber).NotEmpty().Length(12);
        }
    }
}
