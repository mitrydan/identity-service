using Newtonsoft.Json;
using System.Collections.Generic;

namespace IdentityService.BlazorClient.Responses
{
    public sealed class GetUserInfoRs : BaseResponse
    {
        [JsonProperty("role")]
        public IEnumerable<string> Roles { get; set; }

        [JsonProperty("sub")]
        public string Sub { get; set; }
    }
}
