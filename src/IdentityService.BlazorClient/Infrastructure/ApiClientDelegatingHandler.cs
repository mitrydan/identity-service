using Blazored.LocalStorage;
using System;
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

        private HttpClient HttpClient { get; }

        private ISyncLocalStorageService LocalStorage { get; }

        public ApiClientDelegatingHandler(HttpClient httpClient, ISyncLocalStorageService localStorage)
        {
            HttpClient = httpClient;
            LocalStorage = localStorage;

            _accessToken = LocalStorage.GetItemAsString(AccessTokenKey);
            _refreshToken = LocalStorage.GetItemAsString(RefreshTokenKey);
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Console.WriteLine(nameof(ApiClientDelegatingHandler));

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

            return base.SendAsync(request, cancellationToken);
        }
    }
}
