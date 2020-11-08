using Blazored.LocalStorage;
using IdentityService.BlazorClient.Api;
using IdentityService.BlazorClient.Store;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net;
using System.Threading.Tasks;

namespace IdentityService.BlazorClient.Shared
{
    public partial class MainLayout : MainLayoutBase
    {
        private const string AccessTokenKey = "AccessToken";
        private const string RefreshTokenKey = "RefreshToken";

        [Inject]
        private IIdentityServiceClient Client { get; set; }

        [Inject]
        private ILocalStorageService LocalStorage { get; set; }

        [Inject]
        private AuthenticationStateProvider AuthenticationProvider { get; set; }

        private bool IsLoggedId => !string.IsNullOrEmpty(State.UserId);

        public MainLayout()
            : base(nameof(MainLayout))
        { }

        protected override async Task OnInitializedAsync()
        {
            var userInfo = await Client.GetUserInfoAsync();

            if (userInfo.IsFailed && (userInfo.HttpStatusCode == HttpStatusCode.Unauthorized || userInfo.HttpStatusCode == HttpStatusCode.Forbidden))
            {
                return;
            }

            Dispatch(new SetUserIdAndRoleAction(userInfo.Sub, userInfo.Role));
            (AuthenticationProvider as AuthenticationProvider)?.RefreshState();
        }

        private async Task SignOutHandlerAsync()
        {
            await LocalStorage.RemoveItemAsync(AccessTokenKey);
            await LocalStorage.RemoveItemAsync(RefreshTokenKey);
            Dispatch(new SetUserIdAndRoleAction(null, null));
            (AuthenticationProvider as AuthenticationProvider)?.RefreshState();
        }
    }
}
