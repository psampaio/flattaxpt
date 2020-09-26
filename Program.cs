using System;
using System.Net.Http;
using System.Threading.Tasks;
using Blazor.Analytics;
using Blazor.Analytics.GoogleAnalytics;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace FlatTaxPT
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(
                sp => new HttpClient {BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)});

            var trackingId = "UA-179107737-1";
            builder.Services.AddScoped<IAnalytics>(p =>
                new ProductionOnlyAnalytics(
                    ActivatorUtilities.CreateInstance<GoogleAnalyticsStrategy>(p),
                    builder.HostEnvironment, trackingId));

            builder.Services.AddSingleton<ICalculadorImpostosProgressivos, CalculadorImpostosProgressivos>();
            builder.Services.AddSingleton<ICalculadorImpostosFlat, CalculadorImpostosFlat>();

            await builder.Build().RunAsync();
        }
    }
}