
using Financial.Api.Infrastructure.Startup;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddCustomServices();
    })
    .Build();

await host.RunAsync();
