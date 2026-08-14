using JobTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.Common.Interfaces
{
    public interface IJwtTokenService
    {
        /// <summary>Generates a short-lived JWT access token.</summary>
        string GenerateAccessToken(User user, IEnumerable<string> roles);

        /// <summary>Generates a cryptographically random opaque refresh token and its expiry.</summary>
        (string token, DateTime expiresAt) GenerateRefreshToken();

        /// <summary>Returns the access token lifetime in seconds (for the OAuth <c>expires_in</c> field).</summary>
        int AccessTokenExpirySeconds { get; }
    }
}
