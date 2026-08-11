using JobTracker.Domain.Entities;
using JobTracker.Infrastructure.Data;

namespace JobTracker.Seeder
{
    public static class CommonSeeder
    {
        public static async Task Seed(JobTrackerDbContext context) 
        {
            AddOrganization(context);
            AddWorker(context);
            AddCustomer(context);
            await context.SaveChangesAsync();        
        }

        private static void AddOrganization(JobTrackerDbContext context)
        {
            var organization = Organization.Create("Default Organization");
            context.Organizations.Add(organization);
        }

        private static void AddWorker(JobTrackerDbContext context)
        {
            var worker = Worker.Create("John Doe", "john@test.com", "+1234567890");
            context.Workers.Add(worker);
        }

        private static void AddCustomer(JobTrackerDbContext context)
        {
            var customer = Customer.Create("Jane Smith", "jane@test.com", "+0987654321");
            context.Customers.Add(customer);
        }
    }
}
