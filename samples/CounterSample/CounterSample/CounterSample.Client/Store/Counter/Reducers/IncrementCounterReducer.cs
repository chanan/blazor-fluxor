using Blazor.Fluxor;
using CounterSample.Client.Store.Counter.Actions;

namespace CounterSample.Client.Store.Counter.Reducers
{
	public class IncrementCounterReducer : IReducer<CounterState, IncrementCounterAction>
	{
		public CounterState Reduce(CounterState state, IncrementCounterAction action)
		{
			state = state ?? new CounterState(0);
			return new CounterState(state.Value + 1);
		}
	}
}
