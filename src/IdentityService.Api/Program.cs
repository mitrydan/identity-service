using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace IdentityService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseStartup<Startup>()
                        .ConfigureAppConfiguration((hostingContext, config) =>
                        {
                            config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                            config.AddEnvironmentVariables();
                            config.AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true);
                        });
                });
    }
}
