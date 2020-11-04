using System.Collections.Generic;

namespace IdentityService.BlazorClient.Store
{
    public class ApplicationState
    {
        public string UserId { get; set; }

        public IEnumerable<string> UserRoles { get; set; }
    }
}
