using JobTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.Common.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserbyUsername(string username, CancellationToken cancellationToken);
    }
}
