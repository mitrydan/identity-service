namespace IdentityService.BlazorClient.StateManagement
{
    public interface IReducer<TState, TAction>
        where TState : class
        where TAction : IAction
    {
        TState Reduce(TState currentState, TAction action);
    }
}
