using Blazor.Fluxor;

namespace WeatherForecastSample.Client.Store.FetchData.Actions
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
