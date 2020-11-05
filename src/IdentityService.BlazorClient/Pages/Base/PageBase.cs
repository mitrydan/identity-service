using IdentityService.BlazorClient.StateManagement;
using IdentityService.BlazorClient.Store;

namespace IdentityService.BlazorClient.Pages
{
    public class PageBase : StoreComponentBase<ApplicationState, IAction, ApplicationReducer>
    {
        protected PageBase(string componentName)
            : base(componentName)
        { }
    }
}
