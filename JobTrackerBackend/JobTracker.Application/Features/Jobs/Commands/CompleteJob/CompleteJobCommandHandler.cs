using JobTracker.Application.Common.Exceptions;
using JobTracker.Application.Common.Interfaces;
using JobTracker.Domain.Entities;
using JobTracker.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.Features.Jobs.Commands.CompleteJob
{
    internal sealed class CompleteJobCommandHandler(IJobRepository repository) : ICommandHandler<CompleteJobCommand, Unit>
    {
        public async Task<Unit> Handle(CompleteJobCommand request, CancellationToken cancellationToken)
        {
            var job = await repository.GetByIdAsync(request.jobId, cancellationToken);
            if (job == null)
            {
                throw EntityNotFoundException.For<Job>(request.jobId);
            }

            job.UpdateStatus(Status.Completed);
            return Unit.Value;
        }
    }
}
