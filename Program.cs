using System;
using System.Net.Http;
using System.Threading.Tasks;
using Blazor.Analytics;
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

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            if (builder.HostEnvironment.IsProduction())
            {
                builder.Services.AddGoogleAnalytics("UA-179107737-1");
            }

            builder.Services.AddSingleton<ICalculadorImpostosProgressivos, CalculadorImpostosProgressivos>();
            builder.Services.AddSingleton<ICalculadorImpostosFlat, CalculadorImpostosFlat>();

            await builder.Build().RunAsync();
        }
    }
}
