using JobTracker.Application.Common.Interfaces;
using JobTracker.Domain.Entities;
using JobTracker.Domain.Events;
using JobTracker.Infrastructure.Data;
using JobTracker.IntegrationEvents.Events;
using JobTracker.IntegrationEvents.Serializers;

namespace JobTracker.Infrastructure.Integrations
{
    public class OutboxMessagesInterceptor : IOutboxMessagesInterceptor
    {
        private static readonly Dictionary<Type, Func<IDomainEvent, Task<string?>>> handlers = new()
        {
            [typeof(JobCompletedDomainEvent)] = HandleJobCompletedDomainEvent,
        };

        private static async Task<string?> HandleJobCompletedDomainEvent(IDomainEvent domainEvent)
        {
            if (domainEvent is not JobCompletedDomainEvent evt)
                return null;

            var integrated = new JobCompletedIntegrationEvent
            {
                Id = evt.EntityId,
                OccurredOn = evt.OccurredOn
            };

            var result = BasicSerializer.Serialize(integrated);
            return result.SerializedContent;
        }

        public async Task Handle(JobTrackerDbContext context, IDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            var eventType = domainEvent.GetType();
            if (handlers.TryGetValue(eventType, out var handler))
            {
                var type = eventType.FullName ?? string.Empty;

                var content = await handler(domainEvent);
                if (!string.IsNullOrEmpty(content))
                {
                    var outboxMessage = new OutboxMessage
                    {
                        Id = Guid.NewGuid(),
                        OccurredOn = domainEvent.OccurredOn,
                        Type = type,
                        Content = content
                    };

                    await context.OutboxMessages.AddAsync(outboxMessage, cancellationToken);
                }                
            }
        }
    }
}
