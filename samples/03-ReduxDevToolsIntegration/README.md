# 03-ReduxDevToolsIntegration (Integration with Redux Dev Tools)
This sample shows how to integrate with the [Redux dev tools] plugin for Google Chrome. It is recommended that you read both [Tutorial 1] and [Tutorial 2] first.

## Setting up the project
Once you have your project up and running, adding support for ReduxDevTools is simple. Edit the `Program` class within the `Client` project and add `.UseDebugTools()` to the options.
```
var serviceProvider = new BrowserServiceProvider(services =>
{
	services.AddFluxor(options => options
		.UseDependencyInjection(typeof(Program).Assembly)
		.UseDebugTools()
	);
});
```

Next we need to ensure the HTML on the client has the required Javascript to talk to the Redux dev tools. Edit the file `Shared\MainLayout.cshtml` and add the following code to the top of the file

```
@using Blazor.Fluxor.DevTools
@ReduxDevTools.Initialize()
```
## Required changes to state classes
Because the [Redux dev tools] implementation uses serialization to switch back to historial states it is necessary to create a public parameterless constructor on all of your state classes.

```
[Obsolete("For deserialization purposes only. Use the constructor with parameters")]
public CounterState() {}
```

## Temporary steps
1. Currently there is no way in Blazor to instruct `JsonUtil.Deserialize()` to deserialize properties with private setters. Until this is possible you will need to ensure the setter visibility of all state properties is public. (See [Issue 705]).
2. There is currently no way to trigger some kind of a global `StateHasChanged()` to instruct all visible components to rebind their UI to their view state. Until this is possible you will need to descend your components from `FluxorComponent`. (See [Issue 704]). In each `Pages\*.cshtml` file add `@inherits Blazor.Fluxor.Temporary.FluxorComponent`.

[Redux dev tools]: <https://chrome.google.com/webstore/detail/redux-devtools/lmhkpmbekcpmknklioeibfkpmmfibljd>
[Tutorial 1]: <https://github.com/mrpmorris/blazor-fluxor/tree/master/samples/01-CounterSample>
[Tutorial 2]: <https://github.com/mrpmorris/blazor-fluxor/tree/master/samples/02-WeatherForecastSample>
[Issue 704]: <https://github.com/aspnet/Blazor/issues/704>
[Issue 705]: <https://github.com/aspnet/Blazor/issues/705>