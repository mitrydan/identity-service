using Blazored.LocalStorage;
using IdentityService.BlazorClient.Forms;
using IdentityService.BlazorClient.Infrastructure;
using IdentityService.BlazorClient.Requests;
using IdentityService.Common.Constants;
using Microsoft.AspNetCore.Components;
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
        private ISyncLocalStorageService LocalStorage { get; set; }

        private SignInModel SignInModel { get; set; } = new SignInModel();

        public SignIn() : base(nameof(SignIn))
        { }

        private async Task SignInHandlerAsync()
        {
            var response = await Client.GetTokenAsync(new GetTokenRq
            {
                GrantType = "password",
                ClientId = IdentityConstants.AdminServiceName,
                ClientSecret = IdentityConstants.AdminServiceSecret,
                Username = SignInModel.Email,
                Password = SignInModel.Password
            });

            if (response.IsFailed)
            {
                throw new InvalidOperationException("Unable to sign in");
            }

            LocalStorage.SetItem(AccessTokenKey, response.AccessToken);
            LocalStorage.SetItem(RefreshTokenKey, response.RefreshToken);
        }
    }
}
