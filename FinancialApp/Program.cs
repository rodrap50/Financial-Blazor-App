using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace FinancialApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            // ApiBaseUrl is relative ("/") in production, where SWA serves the API same-origin under /api,
            // and absolute (the local Functions host) in Development. Pages call "api/...".
            var apiBaseUrl = builder.Configuration["ApiBaseUrl"]
                ?? throw new InvalidOperationException("ApiBaseUrl is missing from wwwroot/appsettings.json.");
            builder.Services.AddScoped(sp => new HttpClient
            {
                BaseAddress = new Uri(new Uri(builder.HostEnvironment.BaseAddress), apiBaseUrl)
            });

            await builder.Build().RunAsync();
        }
    }
}
