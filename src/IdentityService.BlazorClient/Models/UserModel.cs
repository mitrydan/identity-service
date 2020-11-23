using System;
using System.Collections.Generic;

namespace IdentityService.BlazorClient.Models
{
    public sealed class UserModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}
