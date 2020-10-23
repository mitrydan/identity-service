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

            builder.Services.AddScoped(sp => new IdentityServiceHttpClient());
            builder.Services.AddSingleton(sp => new EventAggregator());

            await builder.Build().RunAsync();
        }
    }
}
