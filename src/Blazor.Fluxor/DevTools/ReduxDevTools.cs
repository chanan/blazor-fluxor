using Blazor.Fluxor.DevTools.CallbackObjects;
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.RenderTree;
using System;
using System.Linq;

namespace Blazor.Fluxor.DevTools
{
	public static class ReduxDevTools
	{
		internal static event EventHandler<JumpToStateCallback> JumpToState;
		internal static event EventHandler AfterJumpToState;

		internal static void DevToolsCallback(string messageAsJson)
		{
			if (string.IsNullOrWhiteSpace(messageAsJson))
				return;

			var message = JsonUtil.Deserialize<BaseCallbackObject>(messageAsJson);
			switch (message?.payload?.type)
			{
				case "JUMP_TO_STATE":
					OnJumpToState(JsonUtil.Deserialize<JumpToStateCallback>(messageAsJson));
					break;
			}
		}

		private static void OnJumpToState(JumpToStateCallback jumpToStateCallback)
		{
			JumpToState?.Invoke(null, jumpToStateCallback);
			AfterJumpToState?.Invoke(null, EventArgs.Empty);
		}

		public static RenderFragment Initialize()
		{
			return (RenderTreeBuilder builder) =>
			{
				if (!ClientOptions.DebugToolsEnabled)
					return;

				string assemblyName = typeof(ReduxDevTools).Assembly.GetName().Name;
				string @namespace = string.Join(".", typeof(ReduxDevTools).FullName.Split('.').Reverse().Skip(1).Reverse().ToArray());
				string className = typeof(ReduxDevTools).Name;
				string methodName = nameof(ReduxDevTools.DevToolsCallback);

				builder.OpenElement(0, "script");
				builder.AddContent(1, $@"
const reduxDevTools = window.__REDUX_DEVTOOLS_EXTENSION__;
if (reduxDevTools !== undefined && reduxDevTools !== null) {{
	const fluxorDevTools = reduxDevTools.connect({{ name: 'Blazor-Fluxor' }});
   if (fluxorDevTools !== undefined && fluxorDevTools !== null) {{
		const fluxorDevToolsCallback = Blazor.platform.findMethod(
			'{assemblyName}',
			'{@namespace}',
			'{className}',
			'{methodName}'
		);
		
		fluxorDevTools.subscribe((message) => {{ 
			const messageAsJson = JSON.stringify(message);
			const messageAsString = Blazor.platform.toDotNetString(messageAsJson);
			Blazor.platform.callMethod(fluxorDevToolsCallback, null, [ messageAsString ]);
		}});
	}}

	fluxorDevTools.init();
	Blazor.registerFunction('fluxorDevTools/dispatch', function(action, state) {{
		fluxorDevTools.send(action, state);
	}});
}}

");

				builder.CloseElement();
			};
		}
	}
}
