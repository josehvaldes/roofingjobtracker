using FluentAssertions;
using JobTracker.Application;
using JobTracker.Domain.Entities;
using NetArchTest.Rules;
using Xunit;

namespace JobTracker.ArchitectureTests
{
    public class ArchitectureTests
    {
        [Fact]
        public void Domain_Should_Not_Depend_On_Infrastructure()
        {
            var result = Types
                .InAssembly(typeof(Job).Assembly)
                .ShouldNot()
                .HaveDependencyOn("JobTracker.Infrastructure")
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Application_Should_Not_Depend_On_Api()
        {
            var result = Types
                .InAssembly(typeof(ApplicationAssemblyMarker).Assembly)
                .ShouldNot()
                .HaveDependencyOn("JobTracker.API")
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Domain_Should_Not_Depend_On_Outer_Layers()
        {
            var result = Types
                .InAssembly(typeof(Job).Assembly)
                .ShouldNot()
                .HaveDependencyOnAny(
                    "JobTracker.API",
                    "JobTracker.Infrastructure")
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }
    }
}
