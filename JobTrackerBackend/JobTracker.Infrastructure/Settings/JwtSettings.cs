using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Infrastructure.Settings
{
    public class JwtSettings
    {
        public static string SectionName = "JwtSettings";

        public string Secret { get; init; } = string.Empty;
        public string Issuer { get; init; } = string.Empty;
        public string Audience { get; init; } = string.Empty;
        public int ExpiryMinutes { get; init; } = 15;
        public int RefreshTokenExpiryDays { get; init; } = 7;
        public int RefreshTokenRetentionDays { get; init; } = 30;

        public string DefaultUsername { get; init; } = "admin";
        public string DefaultPassword { get; init; } = "adminpasswd";
        public string DefaultUserRole { get; init; } = "Admin";
    }
}
