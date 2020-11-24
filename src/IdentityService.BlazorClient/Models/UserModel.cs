using System;
using System.Collections.Generic;
using System.Linq;

namespace IdentityService.BlazorClient.Models
{
    public sealed class UserModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<RoleModel> Roles { get; set; } = Enumerable.Empty<RoleModel>();
    }
}
