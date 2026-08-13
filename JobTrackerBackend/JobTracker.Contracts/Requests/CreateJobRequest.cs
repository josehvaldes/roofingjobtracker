using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Contracts.Requests
{
    public class CreateJobRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public string AssigneeId { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public string OrganizationId { get; set; } = string.Empty;
    }
}
