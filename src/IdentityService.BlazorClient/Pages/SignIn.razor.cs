using Blazored.LocalStorage;
using IdentityService.BlazorClient.Api;
using IdentityService.BlazorClient.Forms;
using IdentityService.BlazorClient.Requests;
using IdentityService.BlazorClient.Responses;
using IdentityService.BlazorClient.Store;
using IdentityService.Common.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Threading.Tasks;

namespace IdentityService.BlazorClient.Pages
{
    public partial class SignIn : PageBase
    {
        private const string AccessTokenKey = "AccessToken";
        private const string RefreshTokenKey = "RefreshToken";

        [Inject]
        private IIdentityServiceClient Client { get; set; }

        [Inject]
        private ILocalStorageService LocalStorage { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private AuthenticationStateProvider AuthenticationProvider { get; set; }

        private SignInModel SignInModel { get; set; } = new SignInModel();

        public SignIn() : base(nameof(SignIn))
        { }

        private async Task SignInHandlerAsync() =>
            await GetTokenAsync(new GetTokenRq
            {
                GrantType = "password",
                ClientId = IdentityConstants.AdminServiceName,
                ClientSecret = IdentityConstants.AdminServiceSecret,
                Username = SignInModel.Email,
                Password = SignInModel.Password
            })
            .ContinueWith(SaveTokenAsync, TaskContinuationOptions.OnlyOnRanToCompletion)
            .ContinueWith(GetUserInfoAsync, TaskContinuationOptions.OnlyOnRanToCompletion)
            .ContinueWith(CompleteSignInAsync, TaskContinuationOptions.OnlyOnRanToCompletion)
            .ContinueWith(t =>
                throw new InvalidOperationException("Unable to sign in"),
                TaskContinuationOptions.OnlyOnFaulted);

        private async Task<GetTokenRs> GetTokenAsync(GetTokenRq request)
        {
            return await Client.GetTokenAsync(request);
        }

        private async Task SaveTokenAsync(Task<GetTokenRs> task)
        {
            var result = await task;
            await LocalStorage.SetItemAsync(AccessTokenKey, result.AccessToken);
            await LocalStorage.SetItemAsync(RefreshTokenKey, result.RefreshToken);
        }

        private async Task<GetUserInfoRs> GetUserInfoAsync(Task task)
        {
            return await Client.GetUserInfoAsync();
        }

        private async Task CompleteSignInAsync(Task<Task<GetUserInfoRs>> task)
        {
            var result = await await task;
            Dispatch(new SetUserIdAndRoleAction(result.Sub, result.Role));
            (AuthenticationProvider as AuthenticationProvider)?.RefreshState();
            NavigationManager.NavigateTo("/");
        }
    }
}
