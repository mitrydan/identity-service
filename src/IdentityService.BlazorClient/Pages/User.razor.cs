using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace IdentityService.BlazorClient.Pages
{
    [Authorize]
    public partial class User : PageBase
    {
        public User() 
            : base(nameof(User))
        { }

        protected override async Task OnInitializedAsync()
        {
            await Task.CompletedTask;
        }
    }
}
