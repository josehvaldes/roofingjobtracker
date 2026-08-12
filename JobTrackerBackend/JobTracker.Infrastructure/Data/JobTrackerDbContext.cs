using JobTracker.Application.Common.Behaviors;
using JobTracker.Domain.Entities;
using JobTracker.Infrastructure.Configurations;
using JobTracker.Infrastructure.Integrations;
using Microsoft.EntityFrameworkCore;

namespace JobTracker.Infrastructure.Data
{
    public class JobTrackerDbContext : DbContext, IUnitOfWork
    {
        private readonly IOutboxMessagesInterceptor? _interceptor;

        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobPhoto> JobPhotos { get; set; }

        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Customer> Customers { get; set; }

        public DbSet<OutboxMessage> OutboxMessages { get; set; }

        public JobTrackerDbContext(DbContextOptions<JobTrackerDbContext> options) : base(options)
        {

        }

        public JobTrackerDbContext(DbContextOptions<JobTrackerDbContext> options, IOutboxMessagesInterceptor interceptor) : base(options)
        {
            _interceptor = interceptor;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(JobTrackerDbContext).Assembly,
                t => t.GetInterfaces().Contains(typeof(IEntityConfiguration))
                );
        }

        public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) 
        {
            var domainEventEntities = ChangeTracker
                .Entries<BaseEntity>()
                .Where(e => e.Entity.DomainEvents.Count > 0)
                .ToList();

            var domainEvents = domainEventEntities
                .SelectMany(e => e.Entity.DomainEvents)
                .ToList();

            domainEventEntities.ForEach(e => e.Entity.ClearDomainEvents());

            if (_interceptor != null)
            {
                var tasks = new List<Task>();
                foreach (var domainEvent in domainEvents)
                {
                    tasks.Add(_interceptor.Handle(domainEvent, cancellationToken));
                }
                await Task.WhenAll(tasks);
            }
            
            var result = await base.SaveChangesAsync(cancellationToken);
            return result;
        }
    }
}
