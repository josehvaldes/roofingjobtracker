using Asp.Versioning;
using FluentValidation;
using JobTracker.API.Settings;
using JobTracker.API.Validators;

namespace JobTracker.API
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddAPIDependencies(
            this IServiceCollection services, IConfiguration config)
        {
            services.AddVersioningConfig(config);
            services.AddCorsConfig(config);


            services.AddValidatorsFromAssembly(typeof(CreateJobRequestValidator).Assembly);

            return services;
        }

        public static IServiceCollection AddVersioningConfig(
           this IServiceCollection services, IConfiguration config)
        {

            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),   // /api/v1/
                    new HeaderApiVersionReader("X-Api-Version")); // optional fallback
            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
            return services;
        }



        public static IServiceCollection AddCorsConfig(this IServiceCollection services, IConfiguration config)
        {
            {
                var corsSettings = config.GetSection(CorsSettings.SectionName)
                                     .Get<CorsSettings>() ?? new CorsSettings();

                services.AddCors(options =>
                {
                    options.AddPolicy("DefaultCors", policy =>
                    {
                        policy
                            .WithOrigins(corsSettings.AllowedOrigins)
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials(); // Remove this if you don't need cookies/auth headers
                    });

                    // Strict policy for sensitive endpoints
                    options.AddPolicy("StrictCors", policy =>
                    {
                        policy
                            .WithOrigins(corsSettings.AllowedOrigins)
                            .WithHeaders("Content-Type", "Authorization")
                            .WithMethods("GET", "POST", "PUT", "DELETE");
                    });
                });
                return services;
            }
        }
    }
}
