using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace IdentityService.Models
{
    public class ApplicationRole : IdentityRole
    {
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }

        public ApplicationRole()
        { }

        public ApplicationRole(string roleName)
            : base(roleName)
        { }
    }
}
