using JobTracker.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.Features.Jobs.Commands.CompleteJob
{
    public sealed record CompleteJobCommand(Guid jobId):ICommand<Unit>
    {

    }
}
