using Blazor.Fluxor;
using ReduxDevToolsIntegration.Client.Store.FetchData.Actions;

namespace ReduxDevToolsIntegration.Client.Store.FetchData.Reducers
{
	public class GetForecastDataSuccessActionReducer : IReducer<FetchDataState, GetForecastDataSuccessAction>
	{
		public FetchDataState Reduce(FetchDataState state, GetForecastDataSuccessAction action)
		{
			return new FetchDataState(
				isLoading: false,
				errorMessage: null,
				forecasts: action.WeatherForecasts);
		}
	}
}
