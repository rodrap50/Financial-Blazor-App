using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(Financial.Api.Startup))]
namespace Financial.Api;
using Data;
using Infrastructure;
using Serilog;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
       // builder.Services.AddHttpClient();
        //throw new System.NotImplementedException();
        string cosmosEndpoint = Environment.GetEnvironmentVariable("CosmosDbEndpoint") ?? "https://localhost:8081";
        string cosmosKey = Environment.GetEnvironmentVariable("CosmosDbKey") ?? "Financials";
        string databaseName = Environment.GetEnvironmentVariable("DatabaseName") ?? "Rodrap50";
        builder.Services.AddDbContext<CosmosDbContext>(options =>
                options.UseCosmos(
                    connectionString: cosmosEndpoint,
                    databaseName: databaseName
                )
            );

        builder.Services.AddScoped<IDatabaseInitializer, DatabaseInitializer>();
        builder.Services.AddScoped<IAccountService, AccountService>();

        builder.Services.AddLogging(logBuilder =>
        {
            logBuilder.AddSerilog(LoggerSetup());
        });
    }

    private ILogger LoggerSetup()
    {
        return new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .CreateLogger();
    }
}
