using IdentityService.BlazorClient.StateManagement;

namespace IdentityService.BlazorClient.Store
{
    public class SetUserIdAction : IAction
    {
        public string UserId { get; private set; }

        public SetUserIdAction(string userId) =>
            UserId = userId;
    }

    public class SetUserRoleAction : IAction
    {
        public string UserRole { get; private set; }

        public SetUserRoleAction(string userRole) =>
            UserRole = userRole;
    }
}
