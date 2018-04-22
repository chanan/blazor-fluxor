namespace Blazor.Fluxor
{
	public interface IFeature<TState>
	{
		TState State { get; }
	}
}
