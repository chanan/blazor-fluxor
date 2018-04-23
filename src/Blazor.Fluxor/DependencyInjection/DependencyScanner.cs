using Blazor.Fluxor.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Blazor.Fluxor.DependencyInjection
{
	internal static class DependencyScanner
	{
		public static IServiceCollection Scan(this IServiceCollection serviceCollection, params Assembly[] assembliesToParse)
		{
			if (assembliesToParse == null || assembliesToParse.Length == 0)
				throw new ArgumentNullException(nameof(assembliesToParse));

			DiscoverReducers(serviceCollection, assembliesToParse, out IEnumerable<DiscoveredReducerInfo> discoveredReducerInfos);
			DiscoverFeatures(serviceCollection, assembliesToParse, discoveredReducerInfos);
			RegisterStore(serviceCollection);
			return serviceCollection;
		}

		private static void RegisterStore(IServiceCollection serviceCollection)
		{
			serviceCollection.AddScoped<IStore, Store>();
		}

		private static void DiscoverReducers(IServiceCollection serviceCollection, Assembly[] assembliesToParse,
			out IEnumerable<DiscoveredReducerInfo> discoveredReducerInfos)
		{
			discoveredReducerInfos = assembliesToParse
				.SelectMany(asm => asm.GetTypes())
				.Select(t => new
				{
					ImplementingType = t,
					GenericParameterTypes = TypeHelper.GetGenericParametersForImplementedInterface(t, typeof(IReducer<,>))
				})
				.Where(x => x.GenericParameterTypes != null)
				.Select(x => new DiscoveredReducerInfo(
					implementingType: x.ImplementingType,
					stateType: x.GenericParameterTypes[0],
					actionType: x.GenericParameterTypes[1]))
				.ToList();

			foreach (DiscoveredReducerInfo discoveredReducerInfo in discoveredReducerInfos)
			{
				RegisterReducer(serviceCollection, discoveredReducerInfo);
			}
		}

		private static void RegisterReducer(IServiceCollection serviceCollection, DiscoveredReducerInfo discoveredReducerInfo)
		{
			serviceCollection.AddScoped(
				serviceType: discoveredReducerInfo.ReducerInterfaceGenericType,
				implementationType: discoveredReducerInfo.ImplementingType);
		}

		private static void DiscoverFeatures(IServiceCollection serviceCollection, Assembly[] assembliesToParse,
			IEnumerable<DiscoveredReducerInfo> discoveredReducerInfos)
		{
			Dictionary<Type, IGrouping<Type, DiscoveredReducerInfo>> discoveredReducerInfosByStateType = discoveredReducerInfos
				.GroupBy(x => x.StateType)
				.ToDictionary(x => x.Key);

			IEnumerable<DiscoveredFeatureInfo> discoveredFeatureInfos = assembliesToParse
				.SelectMany(asm => asm.GetTypes())
				.Select(t => new
				{
					ImplementingType = t,
					GenericParameterTypes = TypeHelper.GetGenericParametersForImplementedInterface(t, typeof(IFeature<>))
				})
				.Where(x => x.GenericParameterTypes != null)
				.Select(x => new DiscoveredFeatureInfo(
					implementingType: x.ImplementingType,
					stateType: x.GenericParameterTypes[0]
					)
				)
				.ToList();

			foreach (DiscoveredFeatureInfo discoveredFeatureInfo in discoveredFeatureInfos)
			{
				discoveredReducerInfosByStateType.TryGetValue(
					discoveredFeatureInfo.StateType,
					out IGrouping<Type, DiscoveredReducerInfo> discoveredFeatureInfosForFeatureState);

				RegisterFeature(
					serviceCollection,
					discoveredFeatureInfo: discoveredFeatureInfo,
					discoveredFeatureInfosForFeatureState: discoveredFeatureInfosForFeatureState);
			}
		}

		private static void RegisterFeature(IServiceCollection serviceCollection,
			DiscoveredFeatureInfo discoveredFeatureInfo, IEnumerable<DiscoveredReducerInfo> discoveredFeatureInfosForFeatureState)
		{
			// Register the implementing type so we can get an instance from the service provider
			serviceCollection.AddScoped(discoveredFeatureInfo.ImplementingType);

			// Register a factory for creating instance of this feature type when requested via the generic IFeature interface
			serviceCollection.AddScoped(discoveredFeatureInfo.FeatureInterfaceGenericType, serviceProvider =>
			{
				// Create an instance of the implementing type
				IFeature featureInstance = (IFeature)serviceProvider.GetService(discoveredFeatureInfo.ImplementingType);

				if (discoveredFeatureInfosForFeatureState != null)
				{
					foreach (DiscoveredReducerInfo reducerInfo in discoveredFeatureInfosForFeatureState)
					{
						IReducer reducerForFeature = (IReducer)serviceProvider.GetService(reducerInfo.ReducerInterfaceGenericType);
						featureInstance.AddReducer(reducerForFeature, reducerInfo.ActionType);
					}
				}

				// Automatically add this feature to the store
				IStore store = serviceProvider.GetService<IStore>();
				store.AddFeature(featureInstance);

				return featureInstance;
			});
		}
	}
}
