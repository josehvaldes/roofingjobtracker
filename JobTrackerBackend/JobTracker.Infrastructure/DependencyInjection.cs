using Hangfire;
using Hangfire.PostgreSql;
using JobTracker.Application.Common.Behaviors;
using JobTracker.Application.Common.Interfaces;
using JobTracker.Infrastructure.Data;
using JobTracker.Infrastructure.Integrations;
using JobTracker.Infrastructure.Jobs;
using JobTracker.Infrastructure.Repositories;
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
            services.AddScoped<IJobRepository, JobRepository>();
            services.AddScoped<ProcessOutboxMessagesJob>();



            services.AddHangfireConfiguration(config);
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
