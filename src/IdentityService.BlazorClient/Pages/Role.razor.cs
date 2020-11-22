using IdentityService.BlazorClient.Api;
using IdentityService.BlazorClient.Models;
using IdentityService.Common.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.BlazorClient.Pages
{
    [Authorize]
    public partial class Role : PageBase
    {
        [Inject]
        private IIdentityServiceClient Client { get; set; }

        private IEnumerable<RoleModel> Roles { get; set; } = Enumerable.Empty<RoleModel>();

        private CreateRoleModel CreateRole { get; set; } = new CreateRoleModel();

        public Role()
            : base(nameof(Role))
        { }

        protected override async Task OnInitializedAsync() =>
            await RefreshAsync(default)
                .ContinueWith(async t => CompleteRefresh(await t), TaskContinuationOptions.OnlyOnRanToCompletion);

        private async Task DeleteHandlerAsync(Guid id) =>
            await Client
                .DeleteRoleAsync(id)
                .ContinueWith(RefreshAsync, TaskContinuationOptions.OnlyOnRanToCompletion)
                .ContinueWith(async t => CompleteRefresh(await await t), TaskContinuationOptions.OnlyOnRanToCompletion);

        private async Task CreateRoleHandlerAsync() =>
            await Client
                .CreateRoleAsync(new CreateRoleRq
                {
                    RoleName = CreateRole.RoleName
                })
                .ContinueWith(RefreshAsync, TaskContinuationOptions.OnlyOnRanToCompletion)
                .ContinueWith(async t => CompleteRefresh(await await t), TaskContinuationOptions.OnlyOnRanToCompletion);

        private async Task<IEnumerable<RoleModel>> RefreshAsync(Task task)
        {
            var getRolesReslt = await Client.GetRolesAsync();

            if (getRolesReslt.IsFailed)
            {
                return Enumerable.Empty<RoleModel>();
            }

            return getRolesReslt.Roles.Select(x => new RoleModel
            {
                Id = x.Id,
                Name = x.Name,
            });
        }

        private Task CompleteRefresh(IEnumerable<RoleModel> roles)
        {
            Roles = roles;
            CreateRole.RoleName = string.Empty;
            StateHasChanged();
            return Task.CompletedTask;
        }
    }
}
