

namespace JobTracker.Contracts.Responses
{
    public class LoginResponse(string accessToken, string username, int expiresIn)
    {
        /// <summary>Short-lived JWT bearer token.</summary>
        public string AccessToken { get; } = accessToken;

        /// <summary>Always "Bearer" — tells the SPA how to attach the token.</summary>
        public string TokenType { get; } = "Bearer";

        /// <summary>Seconds until the access token expires.</summary>
        public int ExpiresIn { get; } = expiresIn;

        /// <summary>The authenticated user's username.</summary>
        public string Username { get; } = username;

    }
}
