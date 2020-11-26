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

        public Task<GetUserInfoRs> GetUserInfoAsync() =>
            GetAsync<GetUserInfoRs>("/connect/userinfo");

        #region Roles

        public Task<GetRolesRs> GetRolesAsync() =>
            GetListAsync<GetRolesRs, RoleRs>("/api/role");

        public Task<bool> CreateRoleAsync(CreateRoleRq request) =>
            PostAsync("/api/role", request);

        public Task<bool> DeleteRoleAsync(Guid id) =>
            DeleteAsync($"/api/role/{id}");

        #endregion

        #region Users

        public Task<GetUsersRs> GetUsersAsync() =>
            GetListAsync<GetUsersRs, UserRs>("/api/user");

        public Task<UserRs> GetUserAsync(Guid id) =>
            GetAsync<UserRs>($"/api/user/{id}");

        public Task<bool> CreateUserAsync(CreateUserRq request) =>
            PostAsync("/api/user", request);

        public Task<bool> DeleteUserAsync(Guid id) =>
            DeleteAsync($"/api/user/{id}");

        public Task<bool> ApplyRoleAsync(Guid id, ApplyRoleRq request) =>
            PutAsync($"/api/user/{id}/applyrole", request);

        public Task<bool> RemoveRoleAsync(Guid id, RemoveRoleRq request) =>
            PutAsync($"/api/user/{id}/removerole", request);

        #endregion

        #region Helper methods

        private async Task<TResponse> GetAsync<TResponse>(string url)
            where TResponse : BaseResponse, new()
        {
            var response = await HttpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TResponse>(responseString);
            }

            return new TResponse
            {
                IsFailed = true,
                HttpStatusCode = response.StatusCode
            };
        }

        private async Task<TResponse> GetListAsync<TResponse, TModel>(string url)
            where TResponse : BaseListResponse<TModel>, new()
            where TModel : class
        {
            var response = await HttpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<IList<TModel>>(responseString);
                return new TResponse
                {
                    Data = result
                };
            }

            return new TResponse
            {
                IsFailed = true,
                HttpStatusCode = response.StatusCode
            };
        }

        private async Task<bool> PostAsync<TRequest>(string url, TRequest request)
            where TRequest : class
        {
            var data = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await HttpClient.PostAsync(url, data);
            return response.IsSuccessStatusCode;
        }

        private async Task<bool> PutAsync<TRequest>(string url, TRequest request)
            where TRequest : class
        {
            var data = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await HttpClient.PutAsync(url, data);
            return response.IsSuccessStatusCode;
        }

        private async Task<bool> DeleteAsync(string url)
        {
            var response = await HttpClient.DeleteAsync(url);
            return response.IsSuccessStatusCode;
        }

        #endregion
    }
}
