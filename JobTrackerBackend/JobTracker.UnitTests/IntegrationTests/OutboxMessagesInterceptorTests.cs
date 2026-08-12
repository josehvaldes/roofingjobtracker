using FluentAssertions;
using JobTracker.Domain.Events;
using JobTracker.Infrastructure.Data;
using JobTracker.Infrastructure.Integrations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JobTracker.UnitTests.IntegrationTests
{
    public class OutboxMessagesInterceptorTests
    {
        [Fact]
        public async Task OutboxMessagesInterceptor_Should_Add_OutboxMessage() 
        {
            var connectionString = "Host=localhost;Port=5432;Database=jobtracker;Username=admin;Password=admintracker";

            var options = new DbContextOptionsBuilder<JobTrackerDbContext>()
                            .UseNpgsql(connectionString)
                            .Options;

            using (var context = new JobTrackerDbContext(options))
            {
                var initCount = context.OutboxMessages.Count();

                var interceptor = new OutboxMessagesInterceptor(context);

                var domainEvent = new JobCompletedDomainEvent(
                    Guid.NewGuid(),
                    DateTime.UtcNow
                    );

                await interceptor.Handle(domainEvent, CancellationToken.None);
                context.SaveChanges();

                var newCount = context.OutboxMessages.Count();
                newCount.Should().Be(initCount + 1);
            }
        }
    }
}
