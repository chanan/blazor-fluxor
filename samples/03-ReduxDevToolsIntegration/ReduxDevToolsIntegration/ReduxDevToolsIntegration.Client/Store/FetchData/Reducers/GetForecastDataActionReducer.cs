using Blazor.Fluxor;
using ReduxDevToolsIntegration.Client.Store.FetchData.Actions;

namespace ReduxDevToolsIntegration.Client.Store.FetchData.Reducers
{
	public class GetForecastDataActionReducer : IReducer<FetchDataState, GetForecastDataAction>
	{
		public FetchDataState Reduce(FetchDataState state, GetForecastDataAction action)
		{
			return new FetchDataState(
				isLoading: true,
				errorMessage: null,
				forecasts: null);
		}
	}
}
