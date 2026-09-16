using System;
using Financial.Api.Infrastructure;
using Financial.Api.Infrastructure.Startup;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();
builder.AddDatabaseConfiguration();
builder.AddSystemServices();

var app = builder.Build();

// Create the Cosmos database/containers if they don't exist before accepting requests.
using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        await scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>().InitializeAsync();
        logger.LogInformation("Cosmos database initialization complete.");
    }
    catch (Exception ex)
    {
        logger.LogCritical(ex, "Cosmos database initialization failed. Host will not start.");
        throw;
    }
}

await app.RunAsync();
