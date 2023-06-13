using BusinessLogic.ViewModels.WorkingShift;
using FluentValidation;

namespace BusinessLogic.Validators.WorkingShift
{
    public class WorkingShiftCreateValidator : AbstractValidator<WorkingShiftCreateModel>
    {
        public WorkingShiftCreateValidator() 
        {
            RuleFor(x => x.ForemanId)
                .NotEmpty();
        }
    }
}
