using Hangfire;
using Hangfire.PostgreSql;
using JobTracker.Application.Common.Behaviors;
using JobTracker.Application.Common.Interfaces;
using JobTracker.Domain.Entities;
using JobTracker.Infrastructure.Auth;
using JobTracker.Infrastructure.Data;
using JobTracker.Infrastructure.Integrations;
using JobTracker.Infrastructure.Jobs;
using JobTracker.Infrastructure.Repositories;
using JobTracker.Infrastructure.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JobTracker.Infrastructure
{
    public static class DependencyInjection
    {
        private static readonly string ConnectionStringName = "DefaultConnection";

        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<JobTrackerDbContext>(options =>
                options.UseNpgsql(config.GetConnectionString(ConnectionStringName)));

            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<JobTrackerDbContext>());
            services.AddScoped<IOutboxMessagesInterceptor, OutboxMessagesInterceptor>();
            services.AddScoped<ProcessOutboxMessagesJob>();

            services.AddScoped<IJobRepository, JobRepository>();
            services.AddScoped<IUserRepository, MockUserRepository>();

            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();


            services.AddHangfireConfiguration(config);


            services.Configure<JwtSettings>(config.GetSection(JwtSettings.SectionName));

            return services;
        }


        private static IServiceCollection AddHangfireConfiguration(this IServiceCollection services, IConfiguration config)
        {
            services.AddHangfire(configuration =>
            {
                configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UsePostgreSqlStorage(options =>
                        options.UseNpgsqlConnection(config.GetConnectionString(ConnectionStringName)));
            });
            services.AddHangfireServer();
            return services;
        }
    }
}
