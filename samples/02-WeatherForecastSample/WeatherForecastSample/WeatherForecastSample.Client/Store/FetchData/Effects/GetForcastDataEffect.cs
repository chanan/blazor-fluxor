using Blazor.Fluxor;
using System.Net.Http;
using WeatherForecastSample.Shared;
using Microsoft.AspNetCore.Blazor;
using WeatherForecastSample.Client.Store.FetchData.Actions;
using System.Threading.Tasks;

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
			System.Console.WriteLine("About to do a HTTP request");
			try
			{
				WeatherForecast[] forecasts = await HttpClient.GetJsonAsync<WeatherForecast[]>("/api/SampleData/WeatherForecasts");
				System.Console.WriteLine("Success");
				return new GetForecastDataSuccessAction(forecasts);
			}
			catch
			{
				System.Console.WriteLine("Failed");
				return new GetForecastDataFailedAction();
			}
		}
	}
}
