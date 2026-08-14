using JobTracker.Application.Common.Interfaces;
using JobTracker.Contracts.Responses;
using JobTracker.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace JobTracker.Application.Features.Auth.Commands
{
    public class LoginCommandHandler(
        IJwtTokenService jwt,
        IPasswordHasher<User> hasher,
        ILogger<LoginCommandHandler> logger,
        IUserRepository userRepository
        ) : ICommandHandler<LoginCommand, LoginResponse>
    {
        
        public async Task<LoginResponse> Handle(LoginCommand cmd, CancellationToken ct) 
        {
            var user = await userRepository.GetUserbyUsername(cmd.Username, ct);
            if (user == null || hasher.VerifyHashedPassword(user, user.PasswordHash, cmd.Password) == PasswordVerificationResult.Failed)
            {
                logger.LogWarning("Failed login attempt for username: {Username}", cmd.Username);
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            var roles = user.UserRoles;
            var accessToken = jwt.GenerateAccessToken(user, roles);
            var (refreshTokenValue, refreshTokenExpiry) = jwt.GenerateRefreshToken();

            return new LoginResponse(accessToken, user.Username, jwt.AccessTokenExpirySeconds);
        }
    }
}
