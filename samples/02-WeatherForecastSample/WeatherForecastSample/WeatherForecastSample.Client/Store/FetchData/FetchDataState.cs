using System.Collections.Generic;
using System.Linq;
using WeatherForecastSample.Shared;

namespace WeatherForecastSample.Client.Store.FetchData
{
    public class FetchDataState
    {
		public bool IsLoading { get; private set; }
		public string ErrorMessage { get; private set; } 
		public WeatherForecast[] Forecasts { get; private set; }

		public FetchDataState(bool isLoading, string errorMessage, IEnumerable<WeatherForecast> forecasts)
		{
			IsLoading = isLoading;
			ErrorMessage = errorMessage;
			Forecasts = forecasts == null ? null : forecasts.ToArray();
		}
	 }
}
