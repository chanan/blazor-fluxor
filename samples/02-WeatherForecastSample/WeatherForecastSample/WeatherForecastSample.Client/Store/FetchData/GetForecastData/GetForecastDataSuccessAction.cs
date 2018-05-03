using Blazor.Fluxor;
using WeatherForecastSample.Shared;

namespace WeatherForecastSample.Client.Store.FetchData.GetForecastData
{
    public class GetForecastDataSuccessAction: IAction
    {
		public WeatherForecast[] WeatherForecasts { get; private set; }

		public GetForecastDataSuccessAction(WeatherForecast[] weatherForecasts)
		{
			WeatherForecasts = weatherForecasts;
		}

    }
}
