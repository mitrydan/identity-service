using IdentityService.BlazorClient.Infrastructure;
using Microsoft.AspNetCore.Components;
using System.Net;
using System.Threading.Tasks;

namespace IdentityService.BlazorClient.Shared
{
    public partial class MainLayout : LayoutComponentBase
    {
        [Inject]
        private IIdentityServiceClient HttpClient { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        private bool IsLoggedId { get; set; }

        private string NavLinkText => IsLoggedId ? "Sign Out" : "Sign In";

        private string NavLinkHref => IsLoggedId ? "signout" : "signin";

        protected override async Task OnInitializedAsync()
        {
            var userInfo = await HttpClient.GetUserInfoAsync();
            
            if (userInfo.IsFailed && (userInfo.HttpStatusCode == HttpStatusCode.Unauthorized || userInfo.HttpStatusCode == HttpStatusCode.Forbidden))
            {
                NavigationManager.NavigateTo("/signin");
                return;
            }

            IsLoggedId = true;
        }
    }
}
