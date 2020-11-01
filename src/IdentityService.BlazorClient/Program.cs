using Blazored.LocalStorage;
using IdentityService.BlazorClient.Infrastructure;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Threading.Tasks;

namespace IdentityService.BlazorClient
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddBlazoredLocalStorage();

            builder.Services.AddSingleton(sp => new EventAggregator());
            builder.Services.AddScoped(sp => new HttpClient());

            builder.Services.AddTransient<ApiClientDelegatingHandler>();
            builder.Services.AddHttpClient("ApiClient").AddHttpMessageHandler<ApiClientDelegatingHandler>();
            builder.Services.AddScoped<IIdentityServiceHttpClient, IdentityServiceHttpClient>();

            await builder.Build().RunAsync();
        }
    }
}
