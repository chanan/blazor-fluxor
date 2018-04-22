namespace Blazor.Fluxor
{
    public interface IReducer<TState, TAction>
    {
		TState Reduce(TAction action, TState state);
    }
}
