using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Contracts.Requests
{
    public class PatchJobRequest
    {
        public string Status { get; set; } = string.Empty;
    }
}
