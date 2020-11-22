using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace IdentityService.BlazorClient.Responses
{
    public sealed class RoleRs
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public sealed class GetRolesRs : BaseResponse
    {
        public List<RoleRs> Roles { get; set; }
    }
}
