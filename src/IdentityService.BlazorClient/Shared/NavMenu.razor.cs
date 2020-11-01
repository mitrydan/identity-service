using Microsoft.AspNetCore.Components;

namespace IdentityService.BlazorClient.Shared
{
    public partial class NavMenu : ComponentBase
    {
        private bool _collapseNavMenu = true;

        [Parameter]
        public bool IsLoggedIn { get; set; }

        private string NavMenuCssClass => _collapseNavMenu ? "collapse" : null;

        private void ToggleNavMenu()
        {
            _collapseNavMenu = !_collapseNavMenu;
        }
    }
}
