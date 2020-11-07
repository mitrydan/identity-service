using Microsoft.AspNetCore.Components;
using System;

namespace IdentityService.BlazorClient.StateManagement
{
    public class StoreLayoutComponentBase<TState, TAction, TReducer> : LayoutComponentBase, IDisposable
        where TState : class
        where TAction : IAction
        where TReducer : IReducer<TState, TAction>
    {
        private readonly string _componentName;

        [Inject]
        private Store<TState, TAction, TReducer> ApplicationStore { get; set; }

        protected TState State => ApplicationStore.ApplicationState;

        protected StoreLayoutComponentBase(string componentName)
        {
            _componentName = componentName;
        }

        protected override void OnInitialized()
        {
            ApplicationStore.Subscribe(_componentName, StateHasChanged);
        }

        protected void Dispatch(TAction action)
        {
            ApplicationStore.Dispatch(action);
        }

        public void Dispose()
        {
            ApplicationStore.Unsubscribe(_componentName);
        }
    }
}
