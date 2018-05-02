using ReduxDevToolsIntegration.Shared;
using System;

namespace ReduxDevToolsIntegration.Client.Store.FetchData
{
	// TODO: Make property setters private - https://github.com/aspnet/Blazor/issues/705
	public class FetchDataState
	{
		public bool IsLoading { get; set; }
		public string ErrorMessage { get; set; }
		public WeatherForecast[] Forecasts { get; set; }

		[Obsolete("For deserialization purposes only. Use the constructor with parameters")]
		public FetchDataState() { } // Required by DevTools to recreate historic state

		public FetchDataState(bool isLoading, string errorMessage, WeatherForecast[] forecasts)
		{
			IsLoading = isLoading;
			ErrorMessage = errorMessage;
			Forecasts = forecasts;
		}
	}
}
