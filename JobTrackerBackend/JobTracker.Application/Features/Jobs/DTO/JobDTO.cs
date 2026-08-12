using JobTracker.Domain.Entities;
using JobTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.Features.Jobs.DTO
{
    public class JobDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public AddressDto Address { get; set; } = null!;

        public string Status { get; set; } = string.Empty;

        public DateTime? ScheduledDate { get; set; }

        public Guid AssigneeId { get; set; }

        public Guid CustomerId { get; set; }

        public Guid OrganizationId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
