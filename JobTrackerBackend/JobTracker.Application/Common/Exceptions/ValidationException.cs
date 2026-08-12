using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace JobTracker.Application.Common.Exceptions
{
    public class ValidationException : Exception
    {
        public IDictionary<string, string[]> Errors { get; }

        public ValidationException(): base("One or more validation failures have occurred.") 
        {
            Errors = new Dictionary<string, string[]>();
        }
        public ValidationException(string message) : base(message) 
        {
            Errors = new Dictionary<string, string[]>();
        }
        public ValidationException(string message, Exception innerException) : base(message, innerException) 
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(IDictionary<string, string[]> errors)
            : base("One or more validation failures have occurred.")
        {
            Errors = new Dictionary<string, string[]>(errors);
        }
    }
}
