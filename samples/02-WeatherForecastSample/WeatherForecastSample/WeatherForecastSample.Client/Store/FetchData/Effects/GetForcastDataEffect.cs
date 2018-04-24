using Blazor.Fluxor;
using System.Net.Http;
using WeatherForecastSample.Shared;
using Microsoft.AspNetCore.Blazor;
using WeatherForecastSample.Client.Store.FetchData.Actions;
using System.Threading.Tasks;
using System;

namespace WeatherForecastSample.Client.Store.FetchData.Effects
{
	public class GetForcastDataEffect : Effect<GetForecastDataAction>
	{
		private readonly HttpClient HttpClient;

		public GetForcastDataEffect(HttpClient httpClient)
		{
			HttpClient = httpClient;
		}

		public override async Task<IAction> Handle(GetForecastDataAction action)
		{
			try
			{
				WeatherForecast[] forecasts = await HttpClient.GetJsonAsync<WeatherForecast[]>("/api/SampleData/WeatherForecasts");
				return new GetForecastDataSuccessAction(forecasts);
			}
			catch (Exception e)
			{
				return new GetForecastDataFailedAction(errorMessage: e.Message);
			}
		}
	}
}
