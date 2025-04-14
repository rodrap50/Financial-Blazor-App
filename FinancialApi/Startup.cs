using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(Financial.Api.Startup))]
namespace Financial.Api;
using Data;
using Infrastructure;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        //throw new System.NotImplementedException();
        string cosmosEndpoint = Environment.GetEnvironmentVariable("CosmosDbEndpoint") ?? "https://localhost:8081";
        string cosmosKey = Environment.GetEnvironmentVariable("CosmosDbKey") ?? "Financials";
        string databaseName = Environment.GetEnvironmentVariable("DatabaseName") ?? "Rodrap50";
        builder.Services.AddDbContext<CosmosDbContext>(options =>
                options.UseCosmos(
                    connectionString: cosmosEndpoint,
                    databaseName: databaseName,
                    cosmosOptionsAction: cosmosOptions =>
                    {
                        // Optional: Configure connection mode
                        // cosmosOptions.ConnectionMode(ConnectionMode.Direct);

                        // Set up the account key
                        //cosmosOptions.AccountKey(cosmosKey);

                        // Configure throughput provisioning (can be at container level too)
                        //cosmosOptions.DefaultRequestUnit(400);

                        // Configure serialization options
                        //cosmosOptions.JsonSerializerOptions(new System.Text.Json.JsonSerializerOptions
                        //{
                        //    PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
                        //    WriteIndented = false
                        //});
                    }
                )
            );

        builder.Services.AddScoped<IDatabaseInitializer, DatabaseInitializer>();
    }
}
