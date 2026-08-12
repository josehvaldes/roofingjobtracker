using JobTracker.Application.Common.Interfaces;
using JobTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.Features.Jobs.Commands.CreateJob
{
    internal sealed class CreateJobCommandHandler(IJobRepository repository) : ICommandHandler<CreateJobCommand, Guid>
    {
        public async Task<Guid> Handle(CreateJobCommand request, CancellationToken cancellationToken)
        {
            var address = new Address(request.StreetAddress, request.City, request.State, request.ZipCode, request.Latitude, request.Longitude);
            var job = Job.CreateJob(
                request.Title, request.Description, address, request.AssigneeId, request.CustomerId, request.OrganizationId
                );

            await repository.AddAsync(job, cancellationToken);

            return job.Id;
        }
    }
}
