using Blazor.Fluxor;
using WeatherForecastSample.Client.Store.FetchData.Actions;

namespace WeatherForecastSample.Client.Store.FetchData.Reducers
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
