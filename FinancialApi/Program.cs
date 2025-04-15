
using Financial.Api.Infrastructure.Startup;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder();

builder.Services.AddCustomServices();
builder.Services.AddFunctionsWorkerDefaults();

var host = builder.Build();
await host.RunAsync();
