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

        #region Roles

        Task<GetRolesRs> GetRolesAsync();

        Task<bool> CreateRoleAsync(CreateRoleRq request);

        Task<bool> DeleteRoleAsync(Guid id);

        #endregion

        #region Users

        Task<GetUsersRs> GetUsersAsync();

        Task<UserRs> GetUserAsync(Guid id);

        Task<bool> CreateUserAsync(CreateUserRq request);

        Task<bool> DeleteUserAsync(Guid id);

        Task<bool> ApplyRoleAsync(Guid id, ApplyRoleRq request);

        Task<bool> RemoveRoleAsync(Guid id, RemoveRoleRq request);

        #endregion
    }
}
