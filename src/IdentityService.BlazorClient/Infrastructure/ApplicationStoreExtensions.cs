using Microsoft.Extensions.DependencyInjection;

namespace IdentityService.BlazorClient.Infrastructure
{
    public static class ApplicationStoreExtensions
    {
        public static void AddApplicationStore<TState, TAction, TReducer>(
            this IServiceCollection services,
            TState initialState,
            TReducer reducer)
            where TState : class
            where TAction : IAction
            where TReducer : IReducer<TState, TAction>
        {
            services.AddSingleton(new ApplicationStore<TState, TAction, TReducer>(initialState, reducer));
        }
    }
}
