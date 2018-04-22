using Blazor.Fluxor;
using CounterSample.Client.Store.Counter.Reducers;

namespace CounterSample.Client.Store.Counter
{
	public class CounterFeature : Feature<CounterState>
	{
		public CounterFeature(IStore store) : base(store) { }

		protected override CounterState GetInitialState() => new CounterState(0);

		protected override void RegisterReducers()
		{
			RegisterReducer(new IncrementCounterReducer());
		}
	}
}
