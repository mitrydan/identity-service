﻿using IdentityService.BlazorClient.Api;
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
    public partial class User : PageBase
    {
        [Inject]
        private IIdentityServiceClient Client { get; set; }

        private IEnumerable<UserModel> Users { get; set; } = Enumerable.Empty<UserModel>();

        private SignInModel CreateUserModel { get; set; } = new SignInModel();

        public User()
            : base(nameof(User))
        { }

        protected override async Task OnInitializedAsync() =>
            await RefreshAsync(default)
                .ContinueWith(async t => CompleteRefresh(await t), TaskContinuationOptions.OnlyOnRanToCompletion);

        private async Task DeleteHandlerAsync(Guid id) =>
            await Client
                .DeleteUserAsync(id)
                .ContinueWith(RefreshAsync, TaskContinuationOptions.OnlyOnRanToCompletion)
                .ContinueWith(async t => CompleteRefresh(await await t), TaskContinuationOptions.OnlyOnRanToCompletion);

        private async Task CreateUserHandlerAsync() =>
            await Client.CreateUserAsync(new CreateUserRq
            {
                UserEmail = CreateUserModel.Email,
                Password = CreateUserModel.Password
            })
                .ContinueWith(RefreshAsync, TaskContinuationOptions.OnlyOnRanToCompletion)
                .ContinueWith(async t => CompleteRefresh(await await t), TaskContinuationOptions.OnlyOnRanToCompletion);

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
                Roles = x.UserRoles?.Select(x => new RoleModel { Id = x.Role.Id, Name = x.Role.Name }) ?? Enumerable.Empty<RoleModel>()
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
