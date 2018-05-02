using Blazor.Fluxor;
using ReduxDevToolsIntegration.Client.Store.Counter.Actions;

namespace ReduxDevToolsIntegration.Client.Store.Counter.Reducers
{
	public class IncrementCounterReducer : IReducer<CounterState, IncrementCounterAction>
	{
		public CounterState Reduce(CounterState state, IncrementCounterAction action)
		{
			return new CounterState(state.Value + 1);
		}
	}
}
