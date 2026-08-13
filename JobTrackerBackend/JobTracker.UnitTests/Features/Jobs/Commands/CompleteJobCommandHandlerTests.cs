using Castle.Core.Resource;
using FluentAssertions;
using JobTracker.Application.Common.Interfaces;
using JobTracker.Application.Features.Jobs.Commands.CompleteJob;
using JobTracker.Domain.Entities;
using JobTracker.Domain.Enums;
using JobTracker.Domain.Exceptions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JobTracker.UnitTests.Features.Jobs.Commands
{
    public class CompleteJobCommandHandlerTests
    {
        public Job CreateJob() 
        {
            var job = Job.CreateJob(
                title: "Fix roofing",
                description: "Fix the leaking roof in the main hall.",
                address: new Address("123 Main St", "Springfield", "IL", "62701", -66.15689, -17.37388),
                assigneeId: Guid.NewGuid(),
                customerId: Guid.NewGuid(),
                organizationId: Guid.NewGuid()
                );

            return job;
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenJobIsNotInProgress()
        {
            var job = CreateJob();
            var jobId = job.Id;
            var mockJobRepository = Substitute.For<IJobRepository>();
            mockJobRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(job);
            var handler = new CompleteJobCommandHandler(mockJobRepository);
            Func<Task> act = async () => await handler.Handle(new CompleteJobCommand(jobId), CancellationToken.None);
            await act.Should().ThrowAsync<InvalidJobTransitionException>()
                .WithMessage($"Job {jobId} cannot transition: Draft jobs can only transition to Scheduled.");
        }

        [Fact]
        public async Task Handle_ShouldCompleteJob_WhenJobExists()
        {
            var job = CreateJob();
            var jobId = job.Id;

            job.UpdateStatus(Status.Scheduled); // Update the job status to Scheduled before completing it
            job.UpdateStatus(Status.InProgress); // Update the job status to InProgress before completing it


            var mockJobRepository = Substitute.For<IJobRepository>();
            mockJobRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(job);

            var handler = new CompleteJobCommandHandler(mockJobRepository);
            await handler.Handle(new CompleteJobCommand(jobId), CancellationToken.None);
            job.Status.Should().Be(Status.Completed);
        }
    }
}
