using Newtonsoft.Json;

namespace IdentityService.BlazorClient.Responses
{
    public sealed class GetUserInfoRs : BaseResponse
    {
        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("sub")]
        public string Sub { get; set; }
    }
}
