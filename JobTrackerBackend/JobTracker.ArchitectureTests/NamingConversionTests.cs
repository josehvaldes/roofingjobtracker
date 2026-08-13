using JobTracker.Application.Common.Interfaces;
using JobTracker.Domain.Entities;
using JobTracker.Domain.Events;
using JobTracker.Infrastructure;
using NetArchTest.Rules;
using Xunit;

namespace JobTracker.ArchitectureTests
{
    public class NamingConversionTests
    {
        [Fact]
        public void DomainEvent_Implementations_Should_Have_Correct_Name()
        {
            var result = Types
                .InAssembly(typeof(Job).Assembly)
                .That()
                .ImplementInterface(typeof(IDomainEvent))
                .Should()
                .HaveNameEndingWith("DomainEvent")
                .GetResult();

            Assert.True(result.IsSuccessful);
        }


        [Fact]
        public void Infrastructe_Respositories_Should_Implement_IRepository()
        {
            var result = Types
                .InAssembly(typeof(InfrastructureAssemblyMarker).Assembly)
                .That()
                .HaveNameEndingWith("JobRepository")
                .Should()
                .ImplementInterface(typeof(IJobRepository))
                .GetResult();

            Assert.True(result.IsSuccessful);
        }
    }
}
