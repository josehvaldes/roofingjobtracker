using JobTracker.Application.Common.Behaviors;
using JobTracker.Domain.Entities;
using JobTracker.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace JobTracker.Infrastructure.Data
{
    public class JobTrackerDbContext : DbContext, IUnitOfWork
    {
        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobPhoto> JobPhotos { get; set; }

        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Customer> Customers { get; set; }

        public DbSet<OutboxMessages> OutboxMessages { get; set; }

        public JobTrackerDbContext(DbContextOptions<JobTrackerDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(JobTrackerDbContext).Assembly,
                t => t.GetInterfaces().Contains(typeof(IEntityConfiguration))
                );
        }
    }
}
