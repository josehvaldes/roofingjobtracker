using FluentValidation;
using JobTracker.Contracts.Requests;

namespace JobTracker.API.Validators
{
    public class PatchJobRequestValidator : AbstractValidator<PatchJobRequest>
    {
        public PatchJobRequestValidator() 
        {
            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required.");
        }
    }
}
