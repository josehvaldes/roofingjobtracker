using JobTracker.Application.Common;
using JobTracker.Application.Common.Interfaces;
using JobTracker.Application.Features.Jobs.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.Features.Jobs.Queries
{
    public sealed record class SearchJobsQuery(string keyWord, int pageNumber, int pageSize): IQuery<PagedList<JobResponse>>
    {
    }
}
