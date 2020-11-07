using Microsoft.AspNetCore.Components;

namespace IdentityService.BlazorClient.Shared
{
    public partial class NavMenu : ComponentBase
    {
        private bool _collapseNavMenu = true;

        private string NavMenuCssClass => _collapseNavMenu ? "collapse" : null;

        [Parameter]
        public bool IsLoggedIn { get; set; }

        private void ToggleNavMenu()
        {
            _collapseNavMenu = !_collapseNavMenu;
        }
    }
}
