using Blazor.Fluxor;

namespace WeatherForecastSample.Client.Store.FetchData
{
	public class FetchDataFeature : Feature<FetchDataState>
	{
		protected override FetchDataState GetInitialState() => new FetchDataState(null);
	}
}
