using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace IdentityService.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
