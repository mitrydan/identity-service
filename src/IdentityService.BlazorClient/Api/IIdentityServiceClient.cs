using IdentityService.BlazorClient.Requests;
using IdentityService.BlazorClient.Responses;
using System.Threading.Tasks;

namespace IdentityService.BlazorClient.Api
{
    public interface IIdentityServiceClient
    {
        Task<GetTokenRs> GetTokenAsync(GetTokenRq request);

        Task<GetUserInfoRs> GetUserInfoAsync();
    }
}
