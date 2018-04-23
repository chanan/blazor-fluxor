using Blazor.Fluxor;

namespace CounterSample.Client.Store.Counter
{
	public class CounterFeature : Feature<CounterState>
	{
		protected override CounterState GetInitialState() => new CounterState(0);
	}
}
