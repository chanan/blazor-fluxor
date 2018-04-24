using Blazor.Fluxor;
using WeatherForecastSample.Client.Store.FetchData.Actions;

namespace WeatherForecastSample.Client.Store.FetchData.Reducers
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
