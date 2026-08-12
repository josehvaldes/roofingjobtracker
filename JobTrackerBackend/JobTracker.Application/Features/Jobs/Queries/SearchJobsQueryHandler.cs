using JobTracker.Application.Common;
using JobTracker.Application.Common.Interfaces;
using JobTracker.Application.Features.Jobs.DTO;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.Features.Jobs.Queries
{
    internal sealed class SearchJobsQueryHandler(IJobRepository repository) : IQueryHandler<SearchJobsQuery, PagedResult<JobDTO>>
    {
        public async Task<PagedResult<JobDTO>> Handle(SearchJobsQuery request, CancellationToken cancellationToken)
        {
            var jobs = await repository.SearchAsync(request.keyWord, request.pageNumber, request.pageSize, cancellationToken);
            var dtoJobs = jobs.Adapt<List<JobDTO>>();
            return new PagedResult<JobDTO>(
                dtoJobs, dtoJobs.Count, request.pageNumber, request.pageSize
                );
        }
    }
}
