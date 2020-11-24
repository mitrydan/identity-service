using IdentityService.BlazorClient.Api;
using IdentityService.BlazorClient.Models;
using IdentityService.BlazorClient.Responses;
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
    public partial class UserDetails : PageBase
    {
        [Inject]
        private IIdentityServiceClient Client { get; set; }

        private IEnumerable<RoleModel> Roles { get; set; } = Enumerable.Empty<RoleModel>();

        private UserModel User { get; set; } = new UserModel();

        private ApplyRoleModel ApplyRole { get; set; } = new ApplyRoleModel();

        [Parameter]
        public Guid Id { get; set; }

        public UserDetails()
            : base(nameof(UserDetails))
        { }

        protected override async Task OnInitializedAsync() =>
            await RefreshAsync(default)
                .ContinueWith(async t => CompleteRefresh(await t), TaskContinuationOptions.OnlyOnRanToCompletion);

        private async Task DeleteHandlerAsync(Guid id) =>
            await Client
                .RemoveRoleAsync(Id, new RemoveRoleRq
                {
                    RoleId = id.ToString()
                })
                .ContinueWith(RefreshAsync, TaskContinuationOptions.OnlyOnRanToCompletion)
                .ContinueWith(async t => CompleteRefresh(await await t), TaskContinuationOptions.OnlyOnRanToCompletion);

        private async Task ApplyRoleHandlerAsync() =>
            await Client
                .ApplyRoleAsync(Id, new ApplyRoleRq
                {
                    RoleId = ApplyRole.RoleId
                })
                .ContinueWith(RefreshAsync, TaskContinuationOptions.OnlyOnRanToCompletion)
                .ContinueWith(async t => CompleteRefresh(await await t), TaskContinuationOptions.OnlyOnRanToCompletion);

        private async Task<(UserRs, GetRolesRs)> RefreshAsync(Task task)
        {
            var getUser = Client.GetUserAsync(Id);
            var getRoles = Client.GetRolesAsync();
            await Task.WhenAll(getUser, getRoles);
            return (await getUser, await getRoles);
        }

        private Task CompleteRefresh((UserRs, GetRolesRs) data)
        {
            User = new UserModel
            {
                Id = data.Item1.Id,
                Name = data.Item1.Name,
                Roles = data.Item1.UserRoles?.Select(x => new RoleModel { Id = x.Role.Id, Name = x.Role.Name }) ?? Enumerable.Empty<RoleModel>()
            };

            Roles = data.Item2.Data
                .Where(x => !User.Roles.Any(ur => ur.Id == x.Id))
                .Select(x => new RoleModel
                {
                    Id = x.Id,
                    Name = x.Name,
                });

            ApplyRole.RoleId = Roles.FirstOrDefault()?.Id.ToString();

            StateHasChanged();
            return Task.CompletedTask;
        }
    }
}
