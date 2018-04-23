using System.Collections.Generic;
using System.Linq;
using WeatherForecastSample.Shared;

namespace WeatherForecastSample.Client.Store.FetchData
{
    public class FetchDataState
    {
		public WeatherForecast[] Forecasts { get; private set; }

		public FetchDataState(IEnumerable<WeatherForecast> forecasts)
		{
			Forecasts = forecasts == null ? null : forecasts.ToArray();
		}
	 }
}
