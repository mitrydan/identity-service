using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace IdentityService.BlazorClient.Infrastructure
{
    public class ApplicationStore<TState, TAction, TReducer>
        where TState : class
        where TAction : IAction
        where TReducer : IReducer<TState, TAction>
    {
        private IDictionary<string, Action> Subscribers { get; }

        private TReducer RootReducer { get; }

        public TState ApplicationState { get; private set; }

        public ApplicationStore(TState initialState, TReducer rootReducer)
        {
            ApplicationState = initialState;
            RootReducer = rootReducer;
            Subscribers = new Dictionary<string, Action>();
        }

        public void Subscribe(string page, Action handler)
        {
            Debug.WriteLine($"Subscribe {page}");

            if (Subscribers.ContainsKey(page))
            {
                Subscribers[page] = handler;
            }
            else
            {
                Subscribers.Add(page, handler);
            }
        }

        public void Unsubscribe(string page)
        {
            Debug.WriteLine($"Unsubscribe {page}");

            if (!Subscribers.ContainsKey(page))
            {
                return;
            }

            Subscribers.Remove(page);
        }

        public void Dispatch(TAction action)
        {
            ApplicationState = RootReducer.Reduce(ApplicationState, action);
            NotifySubscribers();
        }

        private void NotifySubscribers()
        {
            foreach (var handler in Subscribers.Values)
            {
                handler.Invoke();
            }
        }
    }
}
