using IdentityService.BlazorClient.StateManagement;

namespace IdentityService.BlazorClient.Store
{
    public sealed class ApplicationReducer : IReducer<ApplicationState, IAction>
    {
        public ApplicationState Reduce(ApplicationState currentState, IAction action) =>
            new ApplicationState
            {
                UserId = UserIdReducer(currentState.UserId, action),
                UserRole = UserRoleReducer(currentState.UserRole, action)
            };

        private string UserIdReducer(string currentUserId, IAction action) =>
            action switch
            {
                SetUserIdAction a => a.UserId,
                _ => currentUserId
            };

        private string UserRoleReducer(string currentUserRole, IAction action) =>
            action switch
            {
                SetUserRoleAction a => a.UserRole,
                _ => currentUserRole
            };
    }
}
