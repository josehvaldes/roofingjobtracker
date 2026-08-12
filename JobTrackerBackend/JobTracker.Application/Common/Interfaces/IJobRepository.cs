using JobTracker.Application.Common.Behaviors;
using JobTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.Common.Interfaces
{
    public interface IJobRepository
    {
        Task<Job?> GetByIdAsync(Guid jobId, CancellationToken cancellationToken = default);
        Task<Guid> AddAsync (Job job, CancellationToken cancellationToken = default);
        Task<List<Job>> SearchAsync(string keyWord, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    }
}
