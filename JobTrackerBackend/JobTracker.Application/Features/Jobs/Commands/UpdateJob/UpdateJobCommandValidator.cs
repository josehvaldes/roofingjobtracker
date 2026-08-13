using FluentValidation;
using JobTracker.Domain.Enums;

namespace JobTracker.Application.Features.Jobs.Commands.UpdateJob
{
    public class UpdateJobCommandValidator : AbstractValidator<UpdateJobCommand>
    {
        public UpdateJobCommandValidator() 
        {
            RuleFor(x => x.jobId)
                .NotEmpty().WithMessage("Job Id is required.");
            RuleFor(x => x.status)
                .NotEmpty().WithMessage("Status is required.")
                .Must(s => Enum.TryParse<Status>(s, ignoreCase: true, out _))
                .WithMessage($"Status must be one of: {string.Join(", ", Enum.GetNames<Status>())}.");
        }
    }
}
