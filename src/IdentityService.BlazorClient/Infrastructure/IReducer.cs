namespace IdentityService.BlazorClient.Infrastructure
{
    public interface IReducer<TState, TAction>
        where TState : class
        where TAction : IAction
    {
        TState Reduce(TState currentState, TAction action);
    }
}
