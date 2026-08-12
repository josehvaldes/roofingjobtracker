using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.Common.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string message) : base(message) { }

        public static EntityNotFoundException For<T>(Guid id)
        {
            return new EntityNotFoundException($"Entity of type {typeof(T).Name} with ID {id} was not found.");
        }
    }
}
