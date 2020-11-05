using Blazored.LocalStorage;
using IdentityService.BlazorClient.Api;
using IdentityService.BlazorClient.StateManagement;
using IdentityService.BlazorClient.Store;
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

            builder.Services.AddApplicationStore<ApplicationState, IAction, ApplicationReducer>(
                new ApplicationState {},
                new ApplicationReducer());

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
