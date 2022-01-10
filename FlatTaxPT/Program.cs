using Blazor.Analytics;
using FlatTaxPT;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

var trackingId = "UA-179107737-1";
builder.Services.AddGoogleAnalytics(trackingId);

builder.Services.AddSingleton<ICalculadorImpostosProgressivos, CalculadorImpostosProgressivos>();
builder.Services.AddSingleton<ICalculadorImpostosFlat, CalculadorImpostosFlat>();

await builder.Build().RunAsync();