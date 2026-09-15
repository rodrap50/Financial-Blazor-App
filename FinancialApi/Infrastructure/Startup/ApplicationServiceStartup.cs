using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Financial.Api.Infrastructure.Startup;
using System;
using Data;
using Controllers;

public static class ApplicationServiceStartup
{
    private static string cosmosEndpoint = Environment.GetEnvironmentVariable("CosmosDbEndpoint") ?? "https://localhost:8081";
    // Default is the Cosmos DB Emulator's well-known key (same on every install, not a secret).
    private static string cosmosKey = Environment.GetEnvironmentVariable("CosmosDbKey") ?? "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
    private static string databaseName = Environment.GetEnvironmentVariable("DatabaseName") ?? "Rodrap50";
    // "Gateway" is required for the Docker (vNext) emulator; anything else leaves the SDK default (Direct).
    private static bool useGatewayMode = string.Equals(Environment.GetEnvironmentVariable("CosmosDbConnectionMode"), "Gateway", StringComparison.OrdinalIgnoreCase);
    public static IServiceCollection AddCustomServices(this IServiceCollection service)
    {
        service.AddHttpClient();
        service.AddScoped<AccountFunctions>();
        service.AddDbContext<CosmosDbContext>(options =>
                options.UseCosmos(
                    accountEndpoint: cosmosEndpoint,
                    accountKey: cosmosKey,
                    databaseName: databaseName,
                    cosmosOptions =>
                    {
                        if (useGatewayMode)
                        {
                            cosmosOptions.ConnectionMode(ConnectionMode.Gateway);
                            cosmosOptions.LimitToEndpoint();
                        }
                    }
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
