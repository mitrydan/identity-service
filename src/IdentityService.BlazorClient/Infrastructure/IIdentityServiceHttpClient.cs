using IdentityService.BlazorClient.Requests;
using IdentityService.BlazorClient.Responses;
using System.Threading.Tasks;

namespace IdentityService.BlazorClient.Infrastructure
{
    public interface IIdentityServiceHttpClient
    {
        Task<GetTokenRs> GetTokenAsync(GetTokenRq request);

        Task<GetUserInfoRs> GetUserInfoAsync();
    }
}
