using JobTracker.Domain.Events;
using JobTracker.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Infrastructure.Integrations
{
    public interface IOutboxMessagesInterceptor
    {
        Task Handle(JobTrackerDbContext context, IDomainEvent domainEvent, CancellationToken cancellationToken);
    }
}
