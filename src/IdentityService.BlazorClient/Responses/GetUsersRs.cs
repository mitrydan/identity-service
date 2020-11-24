using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace IdentityService.BlazorClient.Responses
{
    public sealed class UserRolesRs
    {
        [JsonProperty("role")]
        public RoleRs Role { get; set; }
    }

    public sealed class UserRs : BaseResponse
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("userName")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("userRoles")]
        public IList<UserRolesRs> UserRoles { get; set; }
    }

    public sealed class GetUsersRs : BaseListResponse<UserRs>
    { }
}
