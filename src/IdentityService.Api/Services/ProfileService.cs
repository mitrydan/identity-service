using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using IdentityService.Models;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityService.Services
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var userId = context?.Subject?.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(userId))
                return;

            var user = await _userManager.FindByIdAsync(userId);
            if (user == default)
                return;

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
            {
                context.IssuedClaims.Add(new Claim(Config.Config.RoleClaimName, userRole));
            }
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            return Task.CompletedTask;
        }
    }
}
