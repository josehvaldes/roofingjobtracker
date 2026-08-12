using FluentAssertions;
using JobTracker.Application.Features.Jobs.Commands.CreateJob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JobTracker.UnitTests.Features.Commands
{
    public class CreateJobCommandValidatorTests
    {
        [Fact]
        public async Task Validate_ShouldNotReturnError_WhenCommandIsValid()
        {
            var createJobCommand = new CreateJobCommand
                (
                    "Fix roof",
                    "Fix the roof of the house",
                    "123 Main St",
                    "Anytown",
                    "CA",
                    "12345",
                    -66.15689,
                    -17.37388,
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid()
                );

            var validator = new CreateJobCommandValidator();

            var result = await validator.ValidateAsync(createJobCommand, TestContext.Current.CancellationToken);
            result.IsValid.Should().BeTrue();
        }
    }
}
