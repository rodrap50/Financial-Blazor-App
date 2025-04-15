using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Financial.Api.Infrastructure.Startup;
using Data;
using System;

public static class ApplicationServiceStartup
{
    private static string cosmosEndpoint = Environment.GetEnvironmentVariable("CosmosDbEndpoint") ?? "https://localhost:8081";
    private static string cosmosKey = Environment.GetEnvironmentVariable("CosmosDbKey") ?? "Financials";
    private static string databaseName = Environment.GetEnvironmentVariable("DatabaseName") ?? "Rodrap50";
    public static IServiceCollection AddCustomServices(this IServiceCollection service)
    {
        service.AddHttpClient();

        service.AddDbContext<CosmosDbContext>(options =>
                options.UseCosmos(
                    connectionString: cosmosEndpoint,
                    databaseName: databaseName
                )
            );

        service.AddScoped<IDatabaseInitializer, DatabaseInitializer>();
        service.AddScoped<IAccountService, AccountService>();

        service.AddLogging(logBuilder =>
        {
            logBuilder.AddSerilog(LoggerSetup());
        });

        return service;
    }
    private static ILogger LoggerSetup()
    {
        return new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .CreateLogger();
    }
}
