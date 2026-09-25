using System;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace Financial.Api.Infrastructure.Controllers;

public class PingFunctions
{
    // Zero-dependency health/runtime probe. Reports the framework the worker is actually running on.
    [Function("Ping")]
    public IActionResult Ping([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ping")] HttpRequest req)
    {
        return new OkObjectResult(new
        {
            framework = RuntimeInformation.FrameworkDescription,
            runtimeVersion = Environment.Version.ToString(),
            workerRuntime = Environment.GetEnvironmentVariable("FUNCTIONS_WORKER_RUNTIME"),
            os = RuntimeInformation.OSDescription,
            utc = DateTime.UtcNow
        });
    }
}
