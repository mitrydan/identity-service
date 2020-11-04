using Blazored.LocalStorage;
using IdentityService.BlazorClient.Responses;
using IdentityService.Common.Constants;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityService.BlazorClient.Infrastructure
{
    public class ApiClientDelegatingHandler : DelegatingHandler
    {
        private const string AccessTokenKey = "AccessToken";
        private const string RefreshTokenKey = "RefreshToken";

        private string _accessToken;
        private string _refreshToken;

        private ISyncLocalStorageService LocalStorage { get; }

        private IConfiguration Configuration { get; }

        public ApiClientDelegatingHandler(
            ISyncLocalStorageService localStorage,
            IConfiguration configuration)
        {
            LocalStorage = localStorage;
            Configuration = configuration;

            _accessToken = LocalStorage.GetItemAsString(AccessTokenKey);
            _refreshToken = LocalStorage.GetItemAsString(RefreshTokenKey);
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                var tokens = await RefreshTokensAsync();
                if (tokens == null)
                {
                    return response;
                }

                SaveTokens(tokens);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
                response = await base.SendAsync(request, cancellationToken);
            }

            return response;
        }

        private async Task<GetTokenRs> RefreshTokensAsync()
        {
            using var httpClient = new HttpClient
            {
                BaseAddress = new Uri(Configuration["App:IdentityServiceUrl"])
            };

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"/connect/token")
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "grant_type", "refresh_token" },
                    { "client_id", IdentityConstants.AdminServiceName },
                    { "client_secret", IdentityConstants.AdminServiceSecret },
                    { "refresh_token", _refreshToken }
                })
            };

            var response = await httpClient.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<GetTokenRs>(responseString);
            }

            return null;
        }

        private void SaveTokens(GetTokenRs tokens)
        {
            _accessToken = tokens.AccessToken;
            _refreshToken = tokens.RefreshToken;

            LocalStorage.SetItem(AccessTokenKey, tokens.AccessToken);
            LocalStorage.SetItem(RefreshTokenKey, tokens.RefreshToken);
        }
    }
}
