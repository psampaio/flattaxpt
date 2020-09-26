using System.Threading.Tasks;
using Blazor.Analytics;
using Blazor.Analytics.GoogleAnalytics;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace FlatTaxPT
{
    public class ProductionOnlyAnalytics : IAnalytics
    {
        private readonly GoogleAnalyticsStrategy analytics;
        private readonly IWebAssemblyHostEnvironment hostEnvironment;

        public ProductionOnlyAnalytics(GoogleAnalyticsStrategy analytics,
            IWebAssemblyHostEnvironment hostEnvironment, string trackingId)
        {
            this.analytics = analytics;
            this.hostEnvironment = hostEnvironment;

            this.analytics.Configure(trackingId);
        }

        public Task Initialize(string trackingId)
        {
            return this.hostEnvironment.IsProduction()
                ? this.analytics.Initialize(trackingId)
                : Task.CompletedTask;
        }

        public Task TrackNavigation(string uri)
        {
            return this.hostEnvironment.IsProduction()
                ? this.analytics.TrackNavigation(uri)
                : Task.CompletedTask;
        }

        public Task TrackEvent(string eventName, string eventValue, string eventCategory = null)
        {
            return this.hostEnvironment.IsProduction()
                ? this.analytics.TrackEvent(eventName, eventValue, eventCategory)
                : Task.CompletedTask;
        }
    }
}