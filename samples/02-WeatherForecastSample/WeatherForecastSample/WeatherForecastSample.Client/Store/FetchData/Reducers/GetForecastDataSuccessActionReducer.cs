using Blazor.Fluxor;
using WeatherForecastSample.Client.Store.FetchData.Actions;

namespace WeatherForecastSample.Client.Store.FetchData.Reducers
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
