namespace JobTracker.API.Settings
{
    public class CorsSettings
    {
        public static string SectionName => "Cors";
        public string[] AllowedOrigins { get; init; } = [];
    }
}
