using Microsoft.AspNetCore.Components;

namespace IdentityService.BlazorClient.Shared
{
    public partial class SurveyPrompt : ComponentBase
    {
        [Parameter]
        public string Title { get; set; }
    }
}
