using JobTracker.Application.Common.Exceptions;
using JobTracker.Application.Common.Interfaces;
using JobTracker.Domain.Entities;
using JobTracker.Domain.Enums;
using MediatR;

namespace JobTracker.Application.Features.Jobs.Commands.UpdateJob
{
    internal sealed class UpdateJobCommandHandler(IJobRepository repository) : ICommandHandler<UpdateJobCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateJobCommand request, CancellationToken cancellationToken)
        {
            var status = Enum.Parse<Status>(request.status, true);
            var job = await repository.GetByIdAsync(request.jobId, cancellationToken);

            if (job == null) 
            {
                throw EntityNotFoundException.For<Job>(request.jobId);
            }

            job.UpdateStatus(status);
            return Unit.Value;
        }
    }
}
