using IdentityService.BlazorClient.StateManagement;
using IdentityService.BlazorClient.Store;

namespace IdentityService.BlazorClient.Shared
{
    public partial class MainLayoutBase : StoreLayoutComponentBase<ApplicationState, IAction, ApplicationReducer>
    {
        protected MainLayoutBase(string componentName)
            : base(componentName)
        { }
    }
}
