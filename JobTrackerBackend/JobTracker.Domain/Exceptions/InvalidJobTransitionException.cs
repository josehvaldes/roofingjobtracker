using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Domain.Exceptions
{
    public class InvalidJobTransitionException : Exception
    {
        public InvalidJobTransitionException(Guid id, string message) : base($"Job {id} cannot transition: {message}") { }
    }
}
