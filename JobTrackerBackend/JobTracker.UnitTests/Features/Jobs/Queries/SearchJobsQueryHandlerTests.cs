using FluentAssertions;
using JobTracker.Application.Common.Interfaces;
using JobTracker.Application.Features.Jobs.Queries;
using JobTracker.Domain.Entities;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JobTracker.UnitTests.Features.Jobs.Queries
{
    public class SearchJobsQueryHandlerTests
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
        public async Task Handle_SearchJobsQuery_ReturnsExpectedResults()
        {
            var mockJobRepository = Substitute.For<IJobRepository>();

            int pageNumber = 1;
            int pageSize = 10;
            var query = new SearchJobsQuery(
                "roof", pageNumber, pageSize
                );

            var expectedJobs = new List<Job>();
            var job1 = CreateJob();
            expectedJobs.Add(job1);

            mockJobRepository.SearchAsync(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<CancellationToken>())
                .Returns(expectedJobs);
            
            var handler = new SearchJobsQueryHandler(mockJobRepository);
            
            var result = await handler.Handle(query, CancellationToken.None);
            result.Should().NotBeNull();
            result.Items.Count.Should().Be(expectedJobs.Count);
            result.PageNumber.Should().Be(pageNumber);
            result.PageSize.Should().Be(pageSize);
        }
    }
}
