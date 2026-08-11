using JobTracker.Domain.Entities;
using JobTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Seeder
{
    public static class JobSeeder
    {

        public static async Task Seed(JobTrackerDbContext context)
        {
            // Implement the seeding logic here
            Console.WriteLine($"Seeding jobs");
            var customer = await context.Customers.FirstOrDefaultAsync();
            var worker = await context.Workers.FirstOrDefaultAsync();
            var organization = await context.Organizations.FirstOrDefaultAsync();

            var jobs = new List<Job>();
            var job = Job.CreateJob(
                title: "Fix roofing",
                description: "Fix the leaking roof in the main hall.",
                address: new Address("123 Main St", "Springfield", "IL", "62701", -66.15689, -17.37388),
                assigneeId: worker?.Id ?? Guid.NewGuid(),
                customerId: customer?.Id ?? Guid.NewGuid(),
                organizationId: organization?.Id ?? Guid.NewGuid()
                );
            job.AddPhoto("https://example.com/photo1.jpg", DateTime.UtcNow, "Initial inspection photo");
            jobs.Add(job);

            jobs.Add(Job.CreateJob(
                title: "Install new HVAC system",
                description: "Install a new HVAC system in the office building.",
                address: new Address("456 Elm St", "Springfield", "IL", "62702", -66.15689, -17.37388),
                assigneeId: worker?.Id ?? Guid.NewGuid(),
                customerId: customer?.Id ?? Guid.NewGuid(),
                organizationId: organization?.Id ?? Guid.NewGuid()
                ));
            context.Jobs.AddRange(jobs);

            await context.SaveChangesAsync();
        }
    }
}
