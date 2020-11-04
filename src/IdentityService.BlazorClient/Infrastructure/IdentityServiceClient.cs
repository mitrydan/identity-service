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
    public class IdentityServiceClient : IIdentityServiceClient
    {
        private HttpClient HttpClient { get; }

        public IdentityServiceClient(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            HttpClient = httpClientFactory.CreateClient("ApiClient");
            HttpClient.BaseAddress = new Uri(configuration["App:IdentityServiceUrl"]);
        }

        public async Task<GetTokenRs> GetTokenAsync(GetTokenRq request)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"/connect/token")
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "grant_type", request.GrantType },
                    { "client_id", request.ClientId },
                    { "client_secret", request.ClientSecret },
                    { "Username", request.Username },
                    { "Password", request.Password }
                })
            };

            var response = await HttpClient.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<GetTokenRs>(responseString);
            }

            return new GetTokenRs
            {
                IsFailed = true,
                HttpStatusCode = response.StatusCode
            };
        }

        public async Task<GetUserInfoRs> GetUserInfoAsync()
        {
            var response = await HttpClient.GetAsync("/connect/userinfo");

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<GetUserInfoRs>(responseString);
            }

            return new GetUserInfoRs
            {
                IsFailed = true,
                HttpStatusCode = response.StatusCode
            };
        }
    }
}
