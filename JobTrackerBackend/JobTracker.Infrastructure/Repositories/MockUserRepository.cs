using JobTracker.Application.Common.Interfaces;
using JobTracker.Domain.Entities;
using JobTracker.Infrastructure.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace JobTracker.Infrastructure.Repositories
{
    /// <summary>
    /// Mock version of IUserRepository for testing purposes. It returns a default user if the username matches the configured default username in JwtSettings.
    /// </summary>
    /// <param name="opts"></param>
    /// <param name="_hasher"></param>
    public class MockUserRepository(
        IOptions<JwtSettings> opts,
        IPasswordHasher<User> _hasher) : IUserRepository
    {
        private readonly JwtSettings _jwtSettings = opts.Value;

        public async Task<User?> GetUserbyUsername(string username, CancellationToken cancellationToken)
        {
            if (username == _jwtSettings.DefaultUsername)
            {
                var user = new User
                {
                    Username = username,
                    UserRoles = new List<string> { _jwtSettings.DefaultUserRole }
                };
                user.PasswordHash = _hasher.HashPassword(user, _jwtSettings.DefaultPassword);
                return user;
            }

            return null;
        }
    }
}
