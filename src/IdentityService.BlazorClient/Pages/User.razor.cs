using IdentityService.BlazorClient.Infrastructure;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace IdentityService.BlazorClient.Pages
{
    public partial class User : ComponentBase
    {
        [Inject]
        private EventAggregator EventAggregator { get; set; }

        [Inject]
        private IdentityServiceHttpClient HttpClient { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await Task.CompletedTask;
        }
    }
}
