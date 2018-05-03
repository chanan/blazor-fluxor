using Blazor.Fluxor;

namespace ReduxDevToolsIntegration.Client.Store.FetchData.GetForecastData
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
