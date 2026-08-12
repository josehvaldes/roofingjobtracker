using JobTracker.Application.Common.Interfaces;
using JobTracker.Application.Features.Jobs.Commands.CreateJob;
using JobTracker.Domain.Entities;
using NSubstitute;
using Xunit;

namespace JobTracker.UnitTests.Features.Commands
{
    public class CreateJobCommandHandlerTests
    {

        [Fact]
        public async Task Handle_ShouldCreateJob_WhenCommandIsValid()
        {
            var repository = Substitute.For<IJobRepository>();
            repository.AddAsync(Arg.Any<Job>(), Arg.Any<CancellationToken>())
                .Returns(ci => ci.Arg<Job>().Id);

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

            var handler = new CreateJobCommandHandler(repository);
            var result = await handler.Handle(createJobCommand, CancellationToken.None);

            Assert.NotEqual(Guid.Empty, result);
            await repository.Received(1).AddAsync(Arg.Any<Job>(), Arg.Any<CancellationToken>());
        }
    }
}
