# 01-CounterSample
This sample shows how to take the standard Visual Studio Blazor template and Fluxorize it.

### Creating the project
1. Create a new Blazor website using the template in Visual Studio (See the [Official Blazor-Fluxor nuget page] for details on how to install Blazor).
2. Name the project `CounterSample`.
3. Add the Blazor-Fluxor nuget package to your Client project. Note that you might have to tick the checkbox `Include prerelease`.
 
### Automatic discovery of store features
1. In the Client project find the `Program.cs` file. 
2. Add `using Blazor.Fluxor;`
3. Change the serviceProvider initialization code to add Fluxor
```
	var serviceProvider = new BrowserServiceProvider(services =>
	{
		services.AddFluxor(options => options
			.UseDependencyInjection(typeof(Program).Assembly)
		);
	});
```

### Adding state
1. In the Client project add a folder named `Store`.
2. It is recommended that you create a folder per feature of your application. Create a folder named `Counter`.
3. Within the `Counter` folder create a file named `CounterState.cs`.
3. Enter the following code. It is good practice to make the properties of state have private setters. This ensures state can only be modified by dispatching an action to the store rather than allowing it to be edited in-place.
```
namespace CounterSample.Client.Store.Counter
{
	public class CounterState
	{
		public int Value { get; private set; }

		public CounterState(int value)
		{
			Value = value;
		}
	}
}
```
4. Now that we have state for the Counter feature we need to create the feature itself. Create a file named `CounterFeature.cs` and enter the following code.
```
using Blazor.Fluxor;

namespace CounterSample.Client.Store.Counter
{
	public class CounterFeature : Feature<CounterState>
	{
		public override string GetName() => "Counter";
		protected override CounterState GetInitialState() => new CounterState(0);
	}
}
```
   * Your class should descend from `Feature<>`, and the type parameter should specify the class you intend to use as the feature's state - in this case the `CounterState` class.
   * Your class should override the `GetName()` method. This should return the name of the property to store this feature's state against.
   * Your class should override the `GetInitialState()` method. This should return the initial state of this feature. This means whatever state you'd like this part of your application to contain *before* the user interacts with it.
 
### Displaying state in the user interface
1. Edit `Pages\Counter.cshtml` and add the following `using` clauses.
```
@using Blazor.Fluxor
@using CounterSample.Client.Store.Counter
@inject IFeature<CounterState> Feature
```
   * `@using Blazor.Fluxor` is required in order to identify the `IFeature<>` interface.
   * `@using CounterSample.Client.Store.Counter` is required to identify the `CounterState` class we wish to use.
   * `@inject IFeature<CounterState> Feature` will instruct Blazor to provide use with a reference to our `CounterFeature` from which we can obtain state.

2. Change the html that displays the value of the counter to display `@Feature.State.Value` instead.

### Dispatching actions to mutate the state
The Flux pattern is structured so that the logic and state of the application can effectively function perfectly fine without a user interface being present at all. Changes to state are executed by using the `Store` to dispatch an `Action` telling it what to do next (update a person's details, increment a counter, or something else).

1. In the Client project's `Store\Counter` folder add a new folder named `Actions`.
2. In the `Actions` folder create a class named `IncrementCounterAction.cs` and add the following code.
```
namespace CounterSample.Client.Store.Counter.Actions
{
	public class IncrementCounterAction: IAction
	{
	}
}
```
   * In more complicated scenarios the action will have properties, in this case we don't need any as the action will always simply increment the current counter by 1.
3. We now need to dispatch an instance of this action to the store whenever the user clicks the `Click me` button. Add the following declarations to the top of the `Pages\Counter.cshtml` file.
```
@using CounterSample.Client.Store.Counter.Actions
@inject IStore Store
```
The declaration section at the top of the file should now look like this:
```
@page "/counter"
@using Blazor.Fluxor
@using CounterSample.Client.Store.Counter
@using CounterSample.Client.Store.Counter.Actions
@inject IStore Store
@inject IFeature<CounterState> Feature
```
   * The `@using CounterSample.Client.Store.Counter.Actions` line is needed to identify the `IncrementCounterAction` class.
   * The `@inject IStore Store` line instructs Blazor to inject the `Store` instance so we can dispatch actions to it.
4. Change the `IncrementCount` function to dispatch an action to the store instructing it to increment the counter value.
```
async void IncrementCount()
{
    await Store.Dispatch(new IncrementCounterAction());
}
```
   * Although the method would work fine without being async, it is best practice to always use the async/await pattern when dispatching to the store because you never know when the store might have an effect registered that will perform asynchronous action.
   
### Mutating the state in response to dispatched actions
So far we have some feature state, a feature that exposes that state for displaying in the user interface, and an action we can dispatch to the store to indicate the user's desire to increment the value in the state. The final piece of the pattern is to implement a `Reducer`.

A `Reducer` is effectively a pure function. It takes the two parameters, the current state and the action dispatched, and it then alters the state according to the property values on the action (in this case there are no properties on the action, so our `Reducer` will always just increment the value by one).

If you recall, earlier I recommended you make all of the properties of your state should have private setters. The recommended pattern for reducing (altering) your state is to replace it with a completely new object that is the same as the previous state but with the relevant changes to its values.

1. Create a folder `Reducers` in the `Store\Counter` folder.
2. In the `Reducers` folder create a new file named `IncrementCounterReducer.cs` and enter the following code.
```
using Blazor.Fluxor;
using CounterSample.Client.Store.Counter.Actions;

namespace CounterSample.Client.Store.Counter.Reducers
{
	public class IncrementCounterReducer : IReducer<CounterState, IncrementCounterAction>
	{
		public CounterState Reduce(CounterState state, IncrementCounterAction action)
		{
			return new CounterState(state.Value + 1);
		}
	}
}
```

   * We import the `Blazor.Fluxor` namespace in order to identify the `IReducer<TState, TActopm>` interface.
   * The first generic parameter in the interface should identify the state the `Reducer` deals with, in this case `CounterState`.
   * The second generic parameter in the interface should identify the action the `Reducer` will react to, in this case it is the `IncrementCounterAction` class.
   
If the `Reducer` does not modify the state at all then it is recommended that you return the original state passed into the `Reduce` method. 
