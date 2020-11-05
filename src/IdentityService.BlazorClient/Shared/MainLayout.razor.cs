using IdentityService.BlazorClient.Api;
using IdentityService.BlazorClient.Store;
using Microsoft.AspNetCore.Components;
using System.Net;
using System.Threading.Tasks;

namespace IdentityService.BlazorClient.Shared
{
    public partial class MainLayout : MainLayoutBase
    {
        [Inject]
        private IIdentityServiceClient Client { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        private bool IsLoggedId => !string.IsNullOrEmpty(State.UserId);

        private string NavLinkText => IsLoggedId ? "Sign Out" : "Sign In";

        private string NavLinkHref => IsLoggedId ? "signout" : "signin";

        public MainLayout() : base(nameof(MainLayout))
        { }

        protected override async Task OnInitializedAsync()
        {
            var userInfo = await Client.GetUserInfoAsync();

            if (userInfo.IsFailed && (userInfo.HttpStatusCode == HttpStatusCode.Unauthorized || userInfo.HttpStatusCode == HttpStatusCode.Forbidden))
            {
                NavigationManager.NavigateTo("/signin");
                return;
            }

            Dispatch(new SetUserIdAndRoleAction(userInfo.Sub, userInfo.Role));
        }
    }
}
