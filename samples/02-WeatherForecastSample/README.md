# 02-WeatherForcastSample (Effects)
This sample shows how to have actions trigger side-effects that can perform asynchronous actions, such as calling out to a server over HTTP to obtain data. If you are not familiar with the basic use of setting up features/state/actions in Fluxor then read [Tutorial 1] first.

## Setting up the project
As with [Tutorial 1] create a basic Blazor app using the template supplied with Visual Studio. Once you have this create the `Store` folder as per the first tutorial. As we are creating modifying the FetchData example create a folder within `Store` named `FetchData` and add a `FetchDataState.cs` class with the following code.
```
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
```

The class basically has 3 pieces of state to it
  * IsLoading: Indicates whether or not the page is waiting for data from the server.
  * ErrorMessage: Shows any kind of unexpected error from the server.
  * Forecasts: The data to show in the UI
 
Then create a file `Store\FetchData\FetchDataFeature.cs` with the following code to declare a feature that uses this state and specifies the initial state for the app to use.
```
using Blazor.Fluxor;

namespace WeatherForecastSample.Client.Store.FetchData
{
	public class FetchDataFeature : Feature<FetchDataState>
	{
		protected override FetchDataState GetInitialState() => new FetchDataState(
			isLoading: false,
			errorMessage: null,
			forecasts: null);
	}
}
```
  
## Creating the action that triggers a data request to the HTTP server
1. Create a class `Store\FetchData\Actions\GetForecastDataAction.cs`. This class can remain empty.
2. When this action is dispatched to the store we want to clear out any previous state and set IsLoading to true. To do this create a class `Store\FetchData\Reducers\GetForecastDataActionReducer.cs` with the following code
```
using Blazor.Fluxor;
using WeatherForecastSample.Client.Store.FetchData.Actions;

namespace WeatherForecastSample.Client.Store.FetchData.Reducers
{
	public class GetForecastDataActionReducer : IReducer<FetchDataState, GetForecastDataAction>
	{
		public FetchDataState Reduce(FetchDataState state, GetForecastDataAction action)
		{
			return new FetchDataState(
				isLoading: true,
				errorMessage: null,
				forecasts: null);
		}
	}
}
```

## Databinding to the feature state
We need this action to be dispatched to the store when the FetchData page is loaded.
1. Edit `Pages\FetchData.cshtml`
2. Set the code at the top of the page to the following
```
@page "/fetchdata"
@using WeatherForecastSample.Shared
@using Blazor.Fluxor
@using Store.FetchData
@using Store.FetchData.Actions
@inject IStore Store
@inject IFeature<FetchDataState> Feature
```
We now need to change the rest of the page in the following ways
   * We show the ErrorMessage if it is set in the state.
   * We show a `Loading` message if IsLoading is true in the state.
   * We databind the table to the forecast data in the feature state

3. At the following code at the top of the page, beneath the *h1* tag
```
@if (Feature.State.ErrorMessage != null)
{
    <h1>Error</h1>
    <p>@Feature.State.ErrorMessage</p>
}
```
4. Replace the section of code that shows the text `Loading...` with the following
```
@if (Feature.State.IsLoading)
{
    <p>Loading...</p>
}
```
5. Change any occurrences of `forecasts` to `Feature.State.Forecasts`. There are two of them, one in an `@if` statement which shows/hides the table, and one in a `@foreach` statement that loops through the data.

## Dispatching the action when the page loads
The code at the bottom of the `FetchData.cshtml` page calls out to a server. We want to move this code out to an effect that is triggered by the `GetForecastDataAction`. So we need to change the code in the `OnInitAsync` method to the following
```
@functions {
    protected override async Task OnInitAsync()
    {
        await Store.Dispatch(new GetForecastDataAction());
    }
}
```

## Listening to the action with an effect, and calling out to a HTTP server asynchronously
Now that our UI has dispatched the `GetForecastDataAction` action we need our store to call out to a HTTP server asychronously and fetch the data we need.

A reducer cannot do this as it should be a pure function that takes a current state and an action, and then immediately returns the new state. Our store shouldn't have to wait for long-running tasks to complete before getting its new state.

The solution is to create an effect that is triggered by the `GetForecastDataAcion` and then performs any long-running tasks in the background so that the store's reducers can complete their work and give the user immediate visual feedback, such as letting them know the page is loading data.

1. Create a file `Store\FetchData\Effects\GetForecastDataEffect.cs`
2. Descend the class from `Effect<GetForecastDataAction>` to indicate which action it should be listening for.
3. Implement the HTTP request like as follows:
```
using Blazor.Fluxor;
using System.Net.Http;
using WeatherForecastSample.Shared;
using Microsoft.AspNetCore.Blazor;
using WeatherForecastSample.Client.Store.FetchData.Actions;
using System.Threading.Tasks;
using System;

namespace WeatherForecastSample.Client.Store.FetchData.Effects
{
	public class GetForecastDataEffect : Effect<GetForecastDataAction>
	{
		private readonly HttpClient HttpClient;

		public GetForecastDataEffect(HttpClient httpClient)
		{
			HttpClient = httpClient;
		}

		public override async Task<IAction[]> Handle(GetForecastDataAction action)
		{
			try
			{
				WeatherForecast[] forecasts = 
				    await HttpClient.GetJsonAsync<WeatherForecast[]>("/api/SampleData/WeatherForecasts");
				return new IAction[] { new GetForecastDataSuccessAction(forecasts) };
			}
			catch (Exception e)
			{
				return new IAction[] { new GetForecastDataFailedAction(errorMessage: e.Message) };
			}
		}
	}
}
```

   * This effect executes a HTTP request to the URL `api/SampleData/WeatherForecasts`
   * It will `await` the response from the server before continuing
   * It will then dispatch a new action to the store depending on whether the request was a success or a failure.
   * Note that the return type is a `Task<>` that returns an array of `IAction`. This is so the effect can return zero to many actions to be dispatched by the store as a side-effect of the action it was triggered by, which in this case is `GetForecastDataAction`.

## Adding the final actions and reducers
In this example, the side effect that executes in a background thread in response to `GetForecastDataAction` indicates either a success or a failure. In your own code you are free to use any pattern you wish (one class for fail & one for success, or a single action with a `bool Success` property, or anything else you can think of).

We now need to add the two actions the `GetForecastDataEffect` can create, and if we need those actions to alter state (as we do in this sample) then we also need reducers.

1. Create a class for the `Failed` action `Store\FetchData\Actions\GetForecastFailedAction.cs` with a single string property `ErrorMessage`.
```
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
```
2. Now create a reducer to alter state accordingly when that action is dispatched. To do this create a file `Store\FetchData\Reducers\GetForecastDataFailedActionReducer.cs` and return a modified state that contains the `ErrorMessage` dispatched in the action.
```
using Blazor.Fluxor;
using WeatherForecastSample.Client.Store.FetchData.Actions;

namespace WeatherForecastSample.Client.Store.FetchData.Reducers
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
```
3. Now create a class for the `Success` action in `Store\FetchData\Actions\GetForecastDataSuccessAction.cs` with a single property that will contain the forecast data the effect received from the server.
```
using Blazor.Fluxor;
using WeatherForecastSample.Shared;

namespace WeatherForecastSample.Client.Store.FetchData.Actions
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
```
4. And, finally, add a reducer to ensure the forecast data is reduced into the state of our `FetchData` feature. Create a class in `Store\FetchData\Reducers\GetForecastDataSuccessAction.cs`
```
using Blazor.Fluxor;
using WeatherForecastSample.Client.Store.FetchData.Actions;

namespace WeatherForecastSample.Client.Store.FetchData.Reducers
{
	public class GetForecastDataSuccessActionReducer : IReducer<FetchDataState, GetForecastDataSuccessAction>
	{
		public FetchDataState Reduce(FetchDataState state, GetForecastDataSuccessAction action)
		{
			return new FetchDataState(
				isLoading: false,
				errorMessage: null,
				forecasts: action.WeatherForecasts);
		}
	}
}
```

## And finally...
Run the application and go to the `Fetch Data` link on the page. You should see the data load from the server. If it is too quick for you to see your `Loading...` message then open the `SampleDataController` in your Server's `Controllers` folder and add `Task.Delay(2000);` at the top of the `WeatherForecasts()` method.

[Tutorial 1]: <https://github.com/mrpmorris/blazor-fluxor/tree/master/samples/01-CounterSample>