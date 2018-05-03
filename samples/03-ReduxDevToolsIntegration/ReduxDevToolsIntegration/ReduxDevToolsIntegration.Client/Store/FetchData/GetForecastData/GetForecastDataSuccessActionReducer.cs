using Blazor.Fluxor;

namespace ReduxDevToolsIntegration.Client.Store.FetchData.GetForecastData
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
