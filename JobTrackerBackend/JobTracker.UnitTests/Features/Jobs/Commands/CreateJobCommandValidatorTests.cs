using FluentAssertions;
using JobTracker.Application.Features.Jobs.Commands.CreateJob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JobTracker.UnitTests.Features.Jobs.Commands
{
    public class CreateJobCommandValidatorTests
    {
        private readonly CreateJobCommandValidator _validator = new();

        private static CreateJobCommand ValidCommand() => new(
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

        [Fact]
        public async Task Validate_ShouldNotReturnError_WhenCommandIsValid()
        {
            var result = await _validator.ValidateAsync(ValidCommand(), TestContext.Current.CancellationToken);
            result.IsValid.Should().BeTrue();
        }

        // Title
        [Fact]
        public async Task Validate_ShouldReturnError_WhenTitleIsEmpty()
        {
            var command = ValidCommand() with { Title = "" };
            var result = await _validator.ValidateAsync(command, TestContext.Current.CancellationToken);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(command.Title));
        }

        [Fact]
        public async Task Validate_ShouldReturnError_WhenTitleExceeds100Characters()
        {
            var command = ValidCommand() with { Title = new string('A', 101) };
            var result = await _validator.ValidateAsync(command, TestContext.Current.CancellationToken);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(command.Title));
        }

        // Description
        [Fact]
        public async Task Validate_ShouldReturnError_WhenDescriptionIsEmpty()
        {
            var command = ValidCommand() with { Description = "" };
            var result = await _validator.ValidateAsync(command, TestContext.Current.CancellationToken);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(command.Description));
        }

        [Fact]
        public async Task Validate_ShouldReturnError_WhenDescriptionExceeds500Characters()
        {
            var command = ValidCommand() with { Description = new string('A', 501) };
            var result = await _validator.ValidateAsync(command, TestContext.Current.CancellationToken);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(command.Description));
        }

        // StreetAddress
        [Fact]
        public async Task Validate_ShouldReturnError_WhenStreetAddressIsEmpty()
        {
            var command = ValidCommand() with { StreetAddress = "" };
            var result = await _validator.ValidateAsync(command, TestContext.Current.CancellationToken);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(command.StreetAddress));
        }

        [Fact]
        public async Task Validate_ShouldReturnError_WhenStreetAddressExceeds200Characters()
        {
            var command = ValidCommand() with { StreetAddress = new string('A', 201) };
            var result = await _validator.ValidateAsync(command, TestContext.Current.CancellationToken);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(command.StreetAddress));
        }

        // City
        [Fact]
        public async Task Validate_ShouldReturnError_WhenCityIsEmpty()
        {
            var command = ValidCommand() with { City = "" };
            var result = await _validator.ValidateAsync(command, TestContext.Current.CancellationToken);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(command.City));
        }

        // State
        [Fact]
        public async Task Validate_ShouldReturnError_WhenStateIsEmpty()
        {
            var command = ValidCommand() with { State = "" };
            var result = await _validator.ValidateAsync(command, TestContext.Current.CancellationToken);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(command.State));
        }

        [Fact]
        public async Task Validate_ShouldReturnError_WhenStateIsNotTwoCharacters()
        {
            var command = ValidCommand() with { State = "California" };
            var result = await _validator.ValidateAsync(command, TestContext.Current.CancellationToken);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(command.State));
        }

        // ZipCode
        [Fact]
        public async Task Validate_ShouldReturnError_WhenZipCodeIsEmpty()
        {
            var command = ValidCommand() with { ZipCode = "" };
            var result = await _validator.ValidateAsync(command, TestContext.Current.CancellationToken);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(command.ZipCode));
        }

        [Fact]
        public async Task Validate_ShouldReturnError_WhenZipCodeHasInvalidFormat()
        {
            var command = ValidCommand() with { ZipCode = "ABCDE" };
            var result = await _validator.ValidateAsync(command, TestContext.Current.CancellationToken);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(command.ZipCode));
        }

        [Fact]
        public async Task Validate_ShouldNotReturnError_WhenZipCodeIsExtendedFormat()
        {
            var command = ValidCommand() with { ZipCode = "12345-6789" };
            var result = await _validator.ValidateAsync(command, TestContext.Current.CancellationToken);
            result.IsValid.Should().BeTrue();
        }

        // Coordinates
        [Theory]
        [InlineData(-91)]
        [InlineData(91)]
        public async Task Validate_ShouldReturnError_WhenLatitudeIsOutOfRange(double latitude)
        {
            var command = ValidCommand() with { Latitude = latitude };
            var result = await _validator.ValidateAsync(command, TestContext.Current.CancellationToken);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(command.Latitude));
        }

        [Theory]
        [InlineData(-181)]
        [InlineData(181)]
        public async Task Validate_ShouldReturnError_WhenLongitudeIsOutOfRange(double longitude)
        {
            var command = ValidCommand() with { Longitude = longitude };
            var result = await _validator.ValidateAsync(command, TestContext.Current.CancellationToken);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(command.Longitude));
        }

        // IDs
        [Fact]
        public async Task Validate_ShouldReturnError_WhenAssigneeIdIsEmpty()
        {
            var command = ValidCommand() with { AssigneeId = Guid.Empty };
            var result = await _validator.ValidateAsync(command, TestContext.Current.CancellationToken);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(command.AssigneeId));
        }

        [Fact]
        public async Task Validate_ShouldReturnError_WhenCustomerIdIsEmpty()
        {
            var command = ValidCommand() with { CustomerId = Guid.Empty };
            var result = await _validator.ValidateAsync(command, TestContext.Current.CancellationToken);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(command.CustomerId));
        }

        [Fact]
        public async Task Validate_ShouldReturnError_WhenOrganizationIdIsEmpty()
        {
            var command = ValidCommand() with { OrganizationId = Guid.Empty };
            var result = await _validator.ValidateAsync(command, TestContext.Current.CancellationToken);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(command.OrganizationId));
        }
    }
}
