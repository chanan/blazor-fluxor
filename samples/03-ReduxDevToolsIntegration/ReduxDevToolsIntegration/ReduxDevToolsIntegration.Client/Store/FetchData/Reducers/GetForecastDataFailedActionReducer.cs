using Blazor.Fluxor;
using ReduxDevToolsIntegration.Client.Store.FetchData.Actions;

namespace ReduxDevToolsIntegration.Client.Store.FetchData.Reducers
{
	public class GetForecastDataFailedActionReducer : IReducer<FetchDataState, GetForecastDataFailedAction>
	{
		public FetchDataState Reduce(FetchDataState state, GetForecastDataFailedAction action)
		{
			return new FetchDataState(
				isLoading: false,
				errorMessage: action.ErrorMessage,
				forecasts: null);
		}
	}
}
