using Blazored.LocalStorage;
using IdentityService.BlazorClient.Infrastructure;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
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
            builder.Services.AddTransient<ApiClientDelegatingHandler>();
            builder.Services
                .AddHttpClient("ApiClient")
                .AddHttpMessageHandler<ApiClientDelegatingHandler>();

            builder.Services.AddScoped<IIdentityServiceClient, IdentityServiceClient>();

            await builder.Build().RunAsync();
        }
    }
}
