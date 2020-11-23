using IdentityService.BlazorClient.Api;
using IdentityService.BlazorClient.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.BlazorClient.Pages
{
    [Authorize]
    public partial class User : PageBase
    {
        [Inject]
        private IIdentityServiceClient Client { get; set; }

        private IEnumerable<UserModel> Users { get; set; } = Enumerable.Empty<UserModel>();

        public User() 
            : base(nameof(User))
        { }

        protected override async Task OnInitializedAsync() =>
            await RefreshAsync(default)
                .ContinueWith(async t => CompleteRefresh(await t), TaskContinuationOptions.OnlyOnRanToCompletion);

        private async Task<IEnumerable<UserModel>> RefreshAsync(Task task)
        {
            var getUsersResult = await Client.GetUsersAsync();

            if (getUsersResult.IsFailed)
            {
                return Enumerable.Empty<UserModel>();
            }

            return getUsersResult.Data.Select(x => new UserModel
            {
                Id = x.Id,
                Name = x.Name,
                Roles = x.UserRoles?.Select(x => x.Role.Name) ?? Enumerable.Empty<string>()
            });
        }

        private Task CompleteRefresh(IEnumerable<UserModel> users)
        {
            Users = users;
            StateHasChanged();
            return Task.CompletedTask;
        }
    }
}
