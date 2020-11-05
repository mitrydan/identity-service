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

namespace IdentityService.BlazorClient.Api
{
    public class ApiClientDelegatingHandler : DelegatingHandler
    {
        private const string AccessTokenKey = "AccessToken";
        private const string RefreshTokenKey = "RefreshToken";

        private ILocalStorageService LocalStorage { get; }

        private IConfiguration Configuration { get; }

        public ApiClientDelegatingHandler(
            ILocalStorageService localStorage,
            IConfiguration configuration)
        {
            LocalStorage = localStorage;
            Configuration = configuration;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accessToken = await LocalStorage.GetItemAsStringAsync(AccessTokenKey);
            var response = await SendRequestAsync(request, cancellationToken, accessToken);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                var tokens = await RefreshTokensAsync();
                if (tokens == null)
                {
                    return response;
                }

                await SaveTokensAsync(tokens);
                response = await SendRequestAsync(request, cancellationToken, tokens.AccessToken);
            }

            return response;
        }

        private Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken, string accessToken)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            return base.SendAsync(request, cancellationToken);
        }

        private async Task<GetTokenRs> RefreshTokensAsync()
        {
            var refreshToken = await LocalStorage.GetItemAsStringAsync(RefreshTokenKey);

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
                    { "refresh_token", refreshToken }
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

        private async Task SaveTokensAsync(GetTokenRs tokens)
        {
            await LocalStorage.SetItemAsync(AccessTokenKey, tokens.AccessToken);
            await LocalStorage.SetItemAsync(RefreshTokenKey, tokens.RefreshToken);
        }
    }
}
