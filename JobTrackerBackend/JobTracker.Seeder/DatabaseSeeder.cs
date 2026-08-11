using JobTracker.Infrastructure.Data;

namespace JobTracker.Seeder
{
    public class DatabaseSeeder
    {
        public static async Task SeedAllAsync(JobTrackerDbContext context)
        {            
            await CommonSeeder.Seed(context);
            await JobSeeder.Seed(context);
        }
    }
}
