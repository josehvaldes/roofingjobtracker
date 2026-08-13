using Hangfire;
using JobTracker.Infrastructure.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Infrastructure
{
    public static class RegisterJobs
    {
        public static void AddBackgroundJobs()
        {
            RecurringJob.AddOrUpdate<ProcessOutboxMessagesJob>(
                "process-outbox-messages",
                job => job.ExecuteAsync(CancellationToken.None),
                Cron.MinuteInterval(5));
        }
    }
}
