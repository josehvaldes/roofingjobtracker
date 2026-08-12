using JobTracker.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.Features.Jobs.Commands.CreateJob
{
    public sealed record CreateJobCommand(string Title,
            string Description,
            string StreetAddress,
            string City,
            string State,
            string ZipCode,
            double Latitude,
            double Longitude,
            Guid AssigneeId,
            Guid CustomerId,
            Guid OrganizationId):ICommand<Guid>
    {
    }
}
