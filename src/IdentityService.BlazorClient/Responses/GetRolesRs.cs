using Newtonsoft.Json;
using System;

namespace IdentityService.BlazorClient.Responses
{
    public sealed class RoleRs
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public sealed class GetRolesRs : BaseListResponse<RoleRs>
    { }
}
