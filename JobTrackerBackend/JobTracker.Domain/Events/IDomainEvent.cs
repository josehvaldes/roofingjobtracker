using MediatR;

namespace JobTracker.Domain.Events
{
    public interface IDomainEvent : INotification
    {
        DateTime OccurredOn { get; }
        Guid EntityId { get; }
    }
}
