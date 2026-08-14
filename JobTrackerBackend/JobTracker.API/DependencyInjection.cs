using Asp.Versioning;
using FluentValidation;
using JobTracker.API.Settings;
using JobTracker.API.Validators;
using JobTracker.Infrastructure.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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


        public static IServiceCollection AddAuthenticationServices(
            this IServiceCollection services, IConfiguration config)
        {

            var jwt = config.GetSection("JwtSettings").Get<JwtSettings>()
                ?? throw new InvalidOperationException("JwtSettings section is missing from configuration.");

            if (string.IsNullOrWhiteSpace(jwt.Secret))
                throw new InvalidOperationException(
                    "JwtSettings:Secret is not configured. " +
                    "Set it via the environment variable: JwtSettings__Secret");
            if (string.IsNullOrWhiteSpace(jwt.Issuer))
                throw new InvalidOperationException(
                    "JwtSettings:Issuer is not configured. " +
                    "Set it via the environment variable: JwtSettings__Issuer");
            if (string.IsNullOrWhiteSpace(jwt.Audience))
                throw new InvalidOperationException(
                    "JwtSettings:Audience is not configured. " +
                    "Set it via the environment variable: JwtSettings__Audience");

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Secret)),
                        ValidateIssuer = true,
                        ValidIssuer = jwt.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwt.Audience,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero  // no tolerance on expiry
                    };
                });

            services.AddAuthorizationBuilder()
                .AddPolicy("AdminOnly", p => p.RequireRole("Admin"))
                .AddPolicy("ManagerOnly", p => p.RequireRole("Admin", "Manager"));
            return services;
        }
    }
}
