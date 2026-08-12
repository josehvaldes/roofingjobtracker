using JobTracker.Application.Common.Exceptions;
using JobTracker.Application.Common.Interfaces;
using JobTracker.Domain.Entities;
using JobTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Infrastructure.Repositories
{
    public class JobRepository(JobTrackerDbContext context) : IJobRepository
    {
        public async Task<Guid> AddAsync(Job job, CancellationToken cancellationToken = default)
        {
            await context.Jobs.AddAsync(job, cancellationToken);
            return job.Id;
        }

        public async Task<Job?> GetByIdAsync(Guid jobId, CancellationToken cancellationToken = default)
        {
            var job = await context.Jobs.FirstOrDefaultAsync(j => j.Id == jobId, cancellationToken);
            if (job == null) 
            { 
                throw EntityNotFoundException.For<Job>(jobId);
            }
            return job;
        }

        public async Task<List<Job>> SearchAsync(string keyWord, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var jobs = await context.Jobs.AsNoTracking()
                .Where(j => j.Title.Contains(keyWord) || j.Description.Contains(keyWord))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
            return jobs;
        }
    }
}
