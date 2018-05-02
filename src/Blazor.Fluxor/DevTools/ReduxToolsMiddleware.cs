using Blazor.Fluxor.DevTools.CallbackObjects;
using Microsoft.AspNetCore.Blazor;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace Blazor.Fluxor.DevTools
{
	internal class ReduxToolsMiddleware : IStoreMiddleware
	{
		private IStore Store;

		public ReduxToolsMiddleware()
		{
			ReduxDevTools.JumpToState += OnJumpToState;
		}

		public void Initialize(IStore store)
		{
			Store = store;
		}

		public void BeforeDispatch<TAction>(IStore store, TAction action) where TAction : IAction
		{
		}

		public void AfterDispatch<TAction>(IStore store, TAction action) where TAction : IAction
		{
			if (!ClientOptions.DebugToolsEnabled)
				return;

			var state = (IDictionary<string, object>)new ExpandoObject();
			foreach (IFeature feature in store.Features.OrderBy(x => x.GetName()))
				state[feature.GetName()] = feature.GetState();

			Microsoft.AspNetCore.Blazor.Browser.Interop.RegisteredFunction.Invoke<object>(
				"fluxorDevTools/dispatch", new ActionInfo(action), state);
		}

		private void OnJumpToState(object sender, JumpToStateCallback e)
		{
			Dictionary<string, IFeature> featuresByName = Store.Features.ToDictionary(x => x.GetName());

			var newFeatureStates = (IDictionary<string, object>) JsonUtil.Deserialize<object>(e.state);
			foreach(KeyValuePair<string, object> newFeatureState in newFeatureStates)
			{
				// Get the feature with the given name
				if (!featuresByName.TryGetValue(newFeatureState.Key, out IFeature feature))
					continue;

				// Get the generic method of JsonUtil.Deserialize<> so we have the correct object type for the state
				string deserializeMethodName = nameof(JsonUtil.Deserialize);
				MethodInfo deserializeMethodInfo = typeof(JsonUtil)
					.GetMethod(deserializeMethodName)
					.GetGenericMethodDefinition()
					.MakeGenericMethod(new Type[] { feature.GetStateType() });

				// Get the state we were given as a json string
				string serializedFeatureState = newFeatureState.Value?.ToString();
				// Deserialize that json using the generic method, so we get an object of the correct type
				object stronglyTypedFeatureState = 
					string.IsNullOrEmpty(serializedFeatureState)
					? null
					: deserializeMethodInfo.Invoke(null, new object[] { serializedFeatureState });

				// Now set the feature's state to the deserialized object
				feature.RestoreState(stronglyTypedFeatureState);
			}
			//TODO: Force render
		}



	}
}
