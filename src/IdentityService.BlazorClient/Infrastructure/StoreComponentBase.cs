using Microsoft.AspNetCore.Components;
using System;

namespace IdentityService.BlazorClient.Infrastructure
{
    public class StoreComponentBase<TState, TAction, TReducer> : ComponentBase, IDisposable
        where TState : class
        where TAction : IAction
        where TReducer : IReducer<TState, TAction>
    {
        private readonly string _componentName;

        [Inject]
        private ApplicationStore<TState, TAction, TReducer> ApplicationStore { get; set; }

        public TState State => ApplicationStore.ApplicationState;

        protected StoreComponentBase(string componentName)
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
