namespace Blazor.Fluxor
{
	public interface IReducer<TState, TAction>
	{
		TState Reduce(TState state, TAction action);
	}
}
