using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace IdentityService.BlazorClient.Pages
{
    [Authorize]
    public partial class Identity : PageBase
    {
        public Identity()
            : base(nameof(Identity))
        { }

        protected override async Task OnInitializedAsync()
        {
            await Task.CompletedTask;
        }
    }
}
