using Hangfire;
using JobTracker.API;
using JobTracker.API.Middleware;
using JobTracker.API.Settings;
using JobTracker.Application;
using JobTracker.Infrastructure;
using JobTracker.Infrastructure.Data;
using JobTracker.Infrastructure.Jobs;
using JobTracker.Seeder;
using Microsoft.EntityFrameworkCore;

MappingConfig.RegisterMappings();

var builder = WebApplication.CreateBuilder(args);

var appSettings = builder.Configuration.GetSection(AppSettings.SectionName)
                                    .Get<AppSettings>() ?? new AppSettings();


builder.Services.AddControllers();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddAPIDependencies(builder.Configuration);
builder.Services.AddApplicationDependencies(builder.Configuration);
builder.Services.AddInfrastructureDependencies(builder.Configuration);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseHangfireDashboard("/hangfire");

//RegisterJobs.AddBackgroundJobs();

app.UseAuthorization();

app.MapControllers();


if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var contextDb = scope.ServiceProvider.GetRequiredService<JobTrackerDbContext>();

    if (appSettings.DbMigration)
    {
        await contextDb.Database.MigrateAsync();
    }

    if (appSettings.LoadSampleData) 
    {
        await DatabaseSeeder.SeedAllAsync(contextDb);
    }
}


app.Run();
