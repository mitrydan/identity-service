using IdentityService.BlazorClient.StateManagement;
using IdentityService.BlazorClient.Store;
using Microsoft.AspNetCore.Components.Authorization;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityService.BlazorClient
{
    public class AuthenticationProvider : AuthenticationStateProvider
    {
        private Store<ApplicationState, IAction, ApplicationReducer> ApplicationStore { get; }

        public AuthenticationProvider(Store<ApplicationState, IAction, ApplicationReducer> applicationStore)
        {
            ApplicationStore = applicationStore;
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var authenticationState = ResolveAuthenticationState();
            var authenticationStateTask = Task.FromResult(authenticationState);

            return authenticationStateTask;
        }

        public void RefreshState()
        {
            var authenticationState = ResolveAuthenticationState();
            var authenticationStateTask = Task.FromResult(authenticationState);

            NotifyAuthenticationStateChanged(authenticationStateTask);
        }

        private AuthenticationState ResolveAuthenticationState()
        {
            var id = ApplicationStore.ApplicationState.UserId;
            var role = ApplicationStore.ApplicationState.UserRole;

            Debug.WriteLine(role);

            ClaimsIdentity claims = null;

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(role))
            {
                claims = new ClaimsIdentity();
                return new AuthenticationState(new ClaimsPrincipal(claims));
            }

            claims = new ClaimsIdentity("Password");
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, id));
            claims.AddClaim(new Claim(ClaimTypes.Role, role));
            return new AuthenticationState(new ClaimsPrincipal(claims));
        }
    }
}
