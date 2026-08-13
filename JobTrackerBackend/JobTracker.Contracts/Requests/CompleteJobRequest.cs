using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Contracts.Requests
{
    public class CompleteJobRequest
    {
        public Guid JobId { get; set; }
    }
}
