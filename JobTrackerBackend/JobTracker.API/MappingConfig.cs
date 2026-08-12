using JobTracker.Application;

namespace JobTracker.API
{
    public static class MappingConfig
    {

        public static void RegisterMappings() {
            // Add more mapping configurations here if needed

            // Register application mappings
            ApplicationMapping.AddApplicationMapping();

        }
    }
}
