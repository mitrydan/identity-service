using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace IdentityService.BlazorClient.Pages
{
    [Authorize]
    public partial class Role : PageBase
    {
        public Role()
            : base(nameof(Role))
        { }

        protected override async Task OnInitializedAsync()
        {
            await Task.CompletedTask;
        }
    }
}
