namespace Blazor.Fluxor
{
	public interface IReducer
	{

	}

	public interface IReducer<TState, TAction>: IReducer
	{
		TState Reduce(TAction action, TState state);
	}
}
