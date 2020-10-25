using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using IdentityService.Config;
using IdentityService.Models;
using IdentityService.Services;

namespace IdentityService
{
    public class Startup
    {
        private const string IdentityServerConnectionString = "IdentityServerConnection";
        private const string IdentityServerUsersConnectionString = "IdentityServerUsersConnection";
        private const string SystemAdminSection = "SystemAdmin";

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc()
                .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore)
                .AddControllersAsServices();

            services.Configure<SystemAdminConfig>(Configuration.GetSection(SystemAdminSection));
            services.AddDbContext<IdentityDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString(IdentityServerConnectionString)));
            services.AddDbContext<AspIdentityDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString(IdentityServerUsersConnectionString)));

            services
                .AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<AspIdentityDbContext>()
                .AddDefaultTokenProviders();

            services
                .AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddConfigurationStore(option =>
                    option.ConfigureDbContext = builder => builder.UseNpgsql(
                        Configuration.GetConnectionString(IdentityServerConnectionString),
                        options => options.MigrationsAssembly("IdentityService.Api")))
                .AddOperationalStore(option =>
                    option.ConfigureDbContext = builder => builder.UseNpgsql(
                        Configuration.GetConnectionString(IdentityServerConnectionString),
                        options => options.MigrationsAssembly("IdentityService.Api")))
                .AddAspNetIdentity<ApplicationUser>()
                .Services.AddTransient<IProfileService, ProfileService>();

            services.AddLocalApiAuthentication();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            DatabaseInitializer.Initialize(app);

            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
