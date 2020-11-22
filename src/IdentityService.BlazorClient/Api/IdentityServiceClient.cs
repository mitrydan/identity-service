using IdentityService.BlazorClient.Requests;
using IdentityService.BlazorClient.Responses;
using IdentityService.Common.Requests;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.BlazorClient.Api
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

        public async Task<GetRolesRs> GetRolesAsync()
        {
            var response = await HttpClient.GetAsync("/api/role");

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                return new GetRolesRs
                {
                    Roles = JsonConvert.DeserializeObject<List<RoleRs>>(responseString)
                };
            }

            return new GetRolesRs
            {
                IsFailed = true,
                HttpStatusCode = response.StatusCode
            };
        }

        public async Task<bool> CreateRoleAsync(CreateRoleRq request)
        {
            var data = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await HttpClient.PostAsync("/api/role", data);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteRoleAsync(Guid id)
        {
            var response = await HttpClient.DeleteAsync($"/api/role/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
