using FluentValidation;
using JobTracker.Contracts.Requests;

namespace JobTracker.API.Validators
{
    public class CreateJobRequestValidator : AbstractValidator<CreateJobRequest>
    {
        public CreateJobRequestValidator() 
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
            RuleFor(x => x.Street)
                .NotEmpty().WithMessage("Street address is required.")
                .MaximumLength(200).WithMessage("Street address cannot exceed 200 characters.");

        }
    }
}
