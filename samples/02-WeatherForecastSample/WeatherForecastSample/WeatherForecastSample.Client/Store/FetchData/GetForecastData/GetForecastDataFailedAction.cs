using Blazor.Fluxor;

namespace WeatherForecastSample.Client.Store.FetchData.GetForecastData
{
    public class GetForecastDataFailedAction: IAction
    {
		public string ErrorMessage { get; private set; }

		public GetForecastDataFailedAction(string errorMessage)
		{
			ErrorMessage = errorMessage;
		}

    }
}
