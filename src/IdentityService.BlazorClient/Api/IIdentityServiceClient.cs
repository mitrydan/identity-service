using IdentityService.BlazorClient.Requests;
using IdentityService.BlazorClient.Responses;
using IdentityService.Common.Requests;
using System;
using System.Threading.Tasks;

namespace IdentityService.BlazorClient.Api
{
    public interface IIdentityServiceClient
    {
        Task<GetTokenRs> GetTokenAsync(GetTokenRq request);

        Task<GetUserInfoRs> GetUserInfoAsync();

        Task<GetRolesRs> GetRolesAsync();

        Task<bool> CreateRoleAsync(CreateRoleRq request);

        Task<bool> DeleteRoleAsync(Guid id);
    }
}
