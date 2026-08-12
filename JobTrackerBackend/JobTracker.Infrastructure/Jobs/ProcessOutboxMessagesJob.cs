using JobTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JobTracker.Infrastructure.Jobs
{
    public class ProcessOutboxMessagesJob(JobTrackerDbContext context)
    {
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var messages = await context.OutboxMessages
                .Where(m => m.ProcessedOn == null).ToListAsync();
            foreach (var message in messages) 
            {
                // Process the message (e.g., send it to a message broker)
                // For demonstration, we'll just mark it as processed
                message.ProcessedOn = DateTime.UtcNow;
            }
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
