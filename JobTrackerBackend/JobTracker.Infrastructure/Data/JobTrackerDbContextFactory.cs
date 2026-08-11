using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace JobTracker.Infrastructure.Data
{
    public class JobTrackerDbContextFactory : IDesignTimeDbContextFactory<JobTrackerDbContext>
    {

        public JobTrackerDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<JobTrackerDbContext>();
            optionsBuilder.UseNpgsql("Host=localhost;Database=JobTracker;Username=admin;Password=admintracker");
            return new JobTrackerDbContext(optionsBuilder.Options);
        }    
    }
}
