using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Domain.Events
{
    public record JobCancelledDomainEvent(Guid id, DateTime time) : IDomainEvent
    {
        public DateTime OccurredOn { get;} = time;

        public Guid EntityId { get; } = id;
    }
}
