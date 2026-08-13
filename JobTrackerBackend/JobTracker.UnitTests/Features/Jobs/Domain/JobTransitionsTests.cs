using FluentAssertions;
using JobTracker.Domain.Entities;
using JobTracker.Domain.Enums;
using JobTracker.Domain.Exceptions;
using Xunit;

namespace JobTracker.UnitTests.Features.Jobs.Domain
{
    public class JobTransitionsTests
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
        public void Job_ShouldHave_DefaultDraftState()
        {
            var job = CreateJob();
            job.Status.Should().Be(Status.Draft);
        }

        [Fact]
        public void Job_ShouldTransitionFromDraftToScheduled_NoExceptions()
        {
            var job = CreateJob();
            job.UpdateStatus(Status.Scheduled);
            job.Status.Should().Be(Status.Scheduled);
        }

        [Fact]
        public void Job_ShouldTransitionFromScheduledToInProgress_NoExceptions()
        {
            var job = CreateJob();
            job.UpdateStatus(Status.Scheduled);
            job.UpdateStatus(Status.InProgress);
            job.Status.Should().Be(Status.InProgress);
        }

        [Fact]
        public void Job_ShouldNotTransitionFromScheduledToInCompleted_throwsExceptions()
        {
            var job = CreateJob();
            job.UpdateStatus(Status.Scheduled);
            Action act = () => job.UpdateStatus(Status.Completed);
            act.Should().Throw<InvalidJobTransitionException>();
        }

        // Draft transitions
        [Fact]
        public void Job_ShouldNotTransitionFromDraftToCancelled_ThrowsException()
        {
            var job = CreateJob();
            Action act = () => job.UpdateStatus(Status.Cancelled);
            act.Should().Throw<InvalidJobTransitionException>();
        }

        [Fact]
        public void Job_ShouldNotTransitionFromDraftToInProgress_ThrowsException()
        {
            var job = CreateJob();
            Action act = () => job.UpdateStatus(Status.InProgress);
            act.Should().Throw<InvalidJobTransitionException>();
        }

        [Fact]
        public void Job_ShouldNotTransitionFromDraftToCompleted_ThrowsException()
        {
            var job = CreateJob();
            Action act = () => job.UpdateStatus(Status.Completed);
            act.Should().Throw<InvalidJobTransitionException>();
        }

        // Scheduled transitions
        [Fact]
        public void Job_ShouldTransitionFromScheduledToCancelled_NoExceptions()
        {
            var job = CreateJob();
            job.UpdateStatus(Status.Scheduled);
            job.UpdateStatus(Status.Cancelled);
            job.Status.Should().Be(Status.Cancelled);
        }

        [Fact]
        public void Job_ShouldNotTransitionFromScheduledToDraft_ThrowsException()
        {
            var job = CreateJob();
            job.UpdateStatus(Status.Scheduled);
            Action act = () => job.UpdateStatus(Status.Draft);
            act.Should().Throw<InvalidJobTransitionException>();
        }

        // InProgress transitions
        [Fact]
        public void Job_ShouldTransitionFromInProgressToCompleted_NoExceptions()
        {
            var job = CreateJob();
            job.UpdateStatus(Status.Scheduled);
            job.UpdateStatus(Status.InProgress);
            job.UpdateStatus(Status.Completed);
            job.Status.Should().Be(Status.Completed);
        }

        [Fact]
        public void Job_ShouldTransitionFromInProgressToCancelled_NoExceptions()
        {
            var job = CreateJob();
            job.UpdateStatus(Status.Scheduled);
            job.UpdateStatus(Status.InProgress);
            job.UpdateStatus(Status.Cancelled);
            job.Status.Should().Be(Status.Cancelled);
        }

        [Fact]
        public void Job_ShouldNotTransitionFromInProgressToScheduled_ThrowsException()
        {
            var job = CreateJob();
            job.UpdateStatus(Status.Scheduled);
            job.UpdateStatus(Status.InProgress);
            Action act = () => job.UpdateStatus(Status.Scheduled);
            act.Should().Throw<InvalidJobTransitionException>();
        }

        // Terminal states
        [Fact]
        public void Job_ShouldNotTransitionFromCompletedToAnyStatus_ThrowsException()
        {
            var job = CreateJob();
            job.UpdateStatus(Status.Scheduled);
            job.UpdateStatus(Status.InProgress);
            job.UpdateStatus(Status.Completed);

            foreach (Status status in Enum.GetValues<Status>())
            {
                if (status == Status.Completed) continue;
                Action act = () => job.UpdateStatus(status);
                act.Should().Throw<InvalidJobTransitionException>(
                    because: $"Completed is a terminal state and cannot transition to {status}");
            }
        }

        [Fact]
        public void Job_ShouldNotTransitionFromCancelledToAnyStatus_ThrowsException()
        {
            var job = CreateJob();
            job.UpdateStatus(Status.Scheduled);
            job.UpdateStatus(Status.Cancelled);

            foreach (Status status in Enum.GetValues<Status>())
            {
                if (status == Status.Cancelled) continue;
                Action act = () => job.UpdateStatus(status);
                act.Should().Throw<InvalidJobTransitionException>(
                    because: $"Cancelled is a terminal state and cannot transition to {status}");
            }
        }

    }
}
