using IdentityService.BlazorClient.Requests;
using IdentityService.BlazorClient.Responses;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace IdentityService.BlazorClient.Infrastructure
{
    public class IdentityServiceHttpClient : IIdentityServiceHttpClient
    {
        private IConfiguration Configuration { get; }

        private HttpClient HttpClient { get; }

        public IdentityServiceHttpClient(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            Configuration = configuration;
            HttpClient = httpClientFactory.CreateClient("ApiClient");
            HttpClient.BaseAddress = new Uri(Configuration["App:IdentityServiceUrl"]);
        }

        public async Task<GetTokenRs> GetTokenAsync(GetTokenRq request)
        {
            var requestData = new Dictionary<string, string> {
                { "grant_type", request.GrantType },
                { "client_id", request.ClientId },
                { "client_secret", request.ClientSecret },
                { "Username", request.Username },
                { "Password", request.Password }
            };
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"/connect/token")
            {
                Content = new FormUrlEncodedContent(requestData)
            };
            var response = await HttpClient.SendAsync(requestMessage);
            var responseString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<GetTokenRs>(responseString);
        }

        public async Task<GetUserInfoRs> GetUserInfoAsync()
        {
            var response = await HttpClient.GetAsync("/connect/userinfo");
            var responseString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<GetUserInfoRs>(responseString);
        }
    }
}
