using Blazor.Fluxor;

namespace WeatherForecastSample.Client.Store.FetchData.GetForecastData
{
	public class GetForecastDataFailedActionReducer : IReducer<FetchDataState, GetForecastDataFailedAction>
	{
		public FetchDataState Reduce(FetchDataState state, GetForecastDataFailedAction action)
		{
			return new FetchDataState(
				isLoading: false,
				errorMessage: action.ErrorMessage,
				forecasts: state.Forecasts);
		}
	}
}
