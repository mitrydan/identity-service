using IdentityService.BlazorClient.StateManagement;

namespace IdentityService.BlazorClient.Store
{
    public class SetUserIdAndRoleAction : IAction
    {
        public string UserId { get; private set; }

        public string UserRole { get; private set; }

        public SetUserIdAndRoleAction(string userId, string userRole)
        {
            UserId = userId;
            UserRole = userRole;
        }
    }
}
