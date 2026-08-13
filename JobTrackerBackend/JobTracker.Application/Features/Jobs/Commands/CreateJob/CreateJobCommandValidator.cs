using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.Features.Jobs.Commands.CreateJob
{
    internal sealed class CreateJobCommandValidator : AbstractValidator<CreateJobCommand>
    {
        public CreateJobCommandValidator() 
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

            RuleFor(x => x.StreetAddress)
                .NotEmpty().WithMessage("Street address is required.")
                .MaximumLength(200).WithMessage("Street address cannot exceed 200 characters.");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City is required.")
                .MaximumLength(100).WithMessage("City cannot exceed 100 characters.");

            RuleFor(x => x.State)
                .NotEmpty().WithMessage("State is required.")
                .Length(2).WithMessage("State must be a 2-character code.");

            RuleFor(x => x.ZipCode)
                .NotEmpty().WithMessage("Zip code is required.")
                .Matches(@"^\d{5}(-\d{4})?$").WithMessage("Zip code must be in the format 12345 or 12345-6789.");

            RuleFor(x => x.Latitude)
                .InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90.");

            RuleFor(x => x.Longitude)
                .InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180.");

            RuleFor(x => x.AssigneeId)
                .NotEmpty().WithMessage("Assignee is required.");

            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("Customer is required.");

            RuleFor(x => x.OrganizationId)
                .NotEmpty().WithMessage("Organization is required.");
        }
    }
}
