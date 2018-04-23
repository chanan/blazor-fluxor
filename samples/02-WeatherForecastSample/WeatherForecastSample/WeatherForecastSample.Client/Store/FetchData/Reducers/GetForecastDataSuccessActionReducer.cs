using Blazor.Fluxor;
using WeatherForecastSample.Client.Store.FetchData.Actions;

namespace WeatherForecastSample.Client.Store.FetchData.Reducers
{
	public class GetForecastDataSuccessActionReducer : IReducer<FetchDataState, GetForecastDataSuccessAction>
	{
		public FetchDataState Reduce(FetchDataState state, GetForecastDataSuccessAction action)
		{
			System.Console.WriteLine("Reducing http success data into state");
			return new FetchDataState(action.WeatherForecasts);
		}
	}
}
