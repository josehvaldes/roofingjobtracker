
using JobTracker.Application.Common.Interfaces;
using MediatR;

namespace JobTracker.Application.Features.Jobs.Commands.UpdateJob
{
    public sealed record UpdateJobCommand(Guid jobId, string status) : ICommand<Unit>
    {
    }
}
