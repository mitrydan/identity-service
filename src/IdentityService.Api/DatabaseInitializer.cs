using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityService.Common.Constants;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using IdentityService.Config;
using IdentityService.Models;
using System;
using System.Linq;

namespace IdentityService
{
    internal class DatabaseInitializer
    {
        public static void Initialize(IApplicationBuilder app)
        {
            using var scope = app
                .ApplicationServices
                .GetService<IServiceScopeFactory>()
                .CreateScope();

            var persistedGrantDbContext = scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>();
            var configurationDbContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
            var identityDbContext = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
            var aspIdentityDbContext = scope.ServiceProvider.GetRequiredService<AspIdentityDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var systemAdminConfig = scope.ServiceProvider.GetRequiredService<IOptions<SystemAdminConfig>>().Value;

            identityDbContext.Database.EnsureCreated();
            InitializeIdentityDatabase(persistedGrantDbContext, configurationDbContext);
            identityDbContext.SaveChanges();

            InitializeIdentityUsersDatabase(aspIdentityDbContext, userManager, roleManager, systemAdminConfig);
        }

        private static void InitializeIdentityDatabase(PersistedGrantDbContext persistedGrantDbContext, ConfigurationDbContext configurationDbContext)
        {
            persistedGrantDbContext.Database.Migrate();
            configurationDbContext.Database.Migrate();

            if (!configurationDbContext.ApiResources.Any())
            {
                foreach (var resource in Config.Config.GetApiResources())
                {
                    configurationDbContext.ApiResources.Add(resource.ToEntity());
                }

                configurationDbContext.SaveChanges();
            }

            if (!configurationDbContext.IdentityResources.Any())
            {
                foreach (var resource in Config.Config.GetIdentityResources())
                {
                    configurationDbContext.IdentityResources.Add(resource.ToEntity());
                }

                configurationDbContext.SaveChanges();
            }

            if (!configurationDbContext.ApiScopes.Any())
            {
                foreach (var apiScope in Config.Config.GetApiScopes())
                {
                    configurationDbContext.ApiScopes.Add(apiScope.ToEntity());
                }

                configurationDbContext.SaveChanges();
            }

            if (!configurationDbContext.Clients.Any())
            {
                foreach (var client in Config.Config.GetClients())
                {
                    configurationDbContext.Clients.Add(client.ToEntity());
                }

                configurationDbContext.SaveChanges();
            }
        }

        private static void InitializeIdentityUsersDatabase(
            AspIdentityDbContext aspIdentityDbContext,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            SystemAdminConfig systemAdminConfig
        )
        {
            aspIdentityDbContext.Database.Migrate();

            if (!aspIdentityDbContext.Roles.Any())
            {
                foreach (IdentityRoles identityRole in Enum.GetValues(typeof(IdentityRoles)))
                {
                    roleManager.CreateAsync(new ApplicationRole(identityRole.ToString()))
                        .GetAwaiter()
                        .GetResult();
                }
            }

            if (!aspIdentityDbContext.Users.Any())
            {
                var user = new ApplicationUser
                {
                    UserName = systemAdminConfig.Email,
                    Email = systemAdminConfig.Email,
                };

                var result = userManager
                    .CreateAsync(user, systemAdminConfig.Password)
                    .GetAwaiter()
                    .GetResult();

                if (!result.Succeeded && !roleManager.RoleExistsAsync(nameof(IdentityRoles.IdentityAdmin)).GetAwaiter().GetResult())
                    return;

                userManager
                    .AddToRoleAsync(user, nameof(IdentityRoles.IdentityAdmin))
                    .GetAwaiter()
                    .GetResult();
            }
        }
    }
}
