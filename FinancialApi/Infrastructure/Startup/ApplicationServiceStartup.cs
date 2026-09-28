using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;

namespace Financial.Api.Infrastructure.Startup;

using System;
using Data;
using Controllers;

public static class ApplicationServiceStartup
{
    private static readonly string CosmosEndpoint =
        Environment.GetEnvironmentVariable("CosmosDbEndpoint") ?? "https://localhost:8081";

    // Default is the Cosmos DB Emulator's well-known key (same on every install, not a secret).
    private static readonly string CosmosKey = Environment.GetEnvironmentVariable("CosmosDbKey") ??
                                               "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

    private static readonly string DatabaseName = Environment.GetEnvironmentVariable("DatabaseName") ?? "Rodrap50";

    // "Gateway" is required for the Docker (vNext) emulator; anything else leaves the SDK default (Direct).
    private static readonly bool UseGatewayMode = string.Equals(Environment.GetEnvironmentVariable("CosmosDbConnectionMode"),
        "Gateway", StringComparison.OrdinalIgnoreCase);

    public static IServiceCollection AddServices(this IServiceCollection service)
    {
        service.AddScoped<IDatabaseInitializer, DatabaseInitializer>()
        .AddScoped<IAccountService, AccountService>()
        .AddScoped<IEventService, EventService>()
        .AddScoped<ITransactionService, TransactionService>();

        service.AddLogging(logBuilder => { logBuilder.AddSerilog(LoggerSetup()); });

        return service;
    }

    private static Logger LoggerSetup()
    {
        return new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .CreateLogger();
    }

    public static IFunctionsWorkerApplicationBuilder AddDatabaseConfiguration(
        this IFunctionsWorkerApplicationBuilder builder)
    {
        builder.Services.AddDbContext<CosmosDbContext>(options =>
            options.UseCosmos(
                accountEndpoint: CosmosEndpoint,
                accountKey: CosmosKey,
                databaseName: DatabaseName,
                cosmosOptions =>
                {
                    if (!UseGatewayMode) return;
                    cosmosOptions.ConnectionMode(ConnectionMode.Gateway);
                    cosmosOptions.LimitToEndpoint();
                }
            )
        );
        return builder;
    }

    public static IFunctionsWorkerApplicationBuilder AddSystemServices(
        this FunctionsApplicationBuilder builder)
    {
        builder.Services.AddHttpClient();

        builder.Services.AddServices();
        return builder;
    }
}
