namespace JobTracker.IntegrationEvents.Events
{
    public class JobCompletedIntegrationEvent
    {
        public Guid Id { get; set; }
        public DateTime OccurredOn { get; set; }


    }
}
