namespace JobTracker.API.Settings
{
    public class AppSettings
    {
        public static string SectionName = "AppSettings";
        public bool DbMigration { get; set; } = false;

        public bool LoadSampleData { get; set; } = false;
    }
}
