using Microsoft.Extensions.DependencyInjection;

namespace IdentityService.BlazorClient.StateManagement
{
    public static class StoreExtensions
    {
        public static void AddApplicationStore<TState, TAction, TReducer>(
            this IServiceCollection services,
            TState initialState,
            TReducer rootReducer
        )
            where TState : class
            where TAction : IAction
            where TReducer : IReducer<TState, TAction>
        {
            services.AddSingleton(new Store<TState, TAction, TReducer>(initialState, rootReducer));
        }
    }
}
