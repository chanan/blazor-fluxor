using Blazor.Fluxor.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Blazor.Fluxor
{
    public static class ServiceCollectionExtensions
    {
		public static IServiceCollection AddFluxor(this IServiceCollection serviceCollection, params Assembly[] assembliesToParse)
		{
			if (assembliesToParse == null || assembliesToParse.Length == 0)
				throw new ArgumentNullException(nameof(assembliesToParse));

			DiscoverReducers(serviceCollection, assembliesToParse, out IEnumerable<DiscoveredReducerInfo> discoveredReducerInfos);
			DiscoverFeatures(serviceCollection, assembliesToParse);
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
			var reducers = assembliesToParse
				.SelectMany(asm => asm.GetTypes())
				.Select(t => new
				{
					ReducerType = t,
					GenericParameterTypes = TypeHelper.GetGenericParametersForSpecificGenericType(t, typeof(IReducer<,>))
				})
				.Where(x => x.GenericParameterTypes != null)
				.ToList();

			reducers.ForEach(x => RegisterReducer(
				serviceCollection,
				reducerImplementationType: x.ReducerType,
				stateType: x.GenericParameterTypes[0],
				actionType: x.GenericParameterTypes[1]));

			discoveredReducerInfos = reducers.Select(x => new DiscoveredReducerInfo(
				stateType: x.GenericParameterTypes[0],
				actionType: x.GenericParameterTypes[1]));
		}

		private static void RegisterReducer(IServiceCollection serviceCollection, 
			Type reducerImplementationType, Type stateType, Type actionType)
		{
			// Create the generic type we need to register
			Type reducerInterfaceType = typeof(IReducer<,>).MakeGenericType(stateType, actionType);
			serviceCollection.AddScoped(reducerInterfaceType, reducerImplementationType);
		}

		private static void DiscoverFeatures(IServiceCollection serviceCollection, Assembly[] assembliesToParse)
		{
			var features = assembliesToParse
				.SelectMany(asm => asm.GetTypes())
				.Select(t => new
				{
					FeatureType = t,
					GenericParameterTypes = TypeHelper.GetGenericParametersForSpecificGenericType(t, typeof(Feature<>))
				})
				.Where(x => x.GenericParameterTypes != null)
				.ToList();
			features.ForEach(x => RegisterFeature(serviceCollection, x.FeatureType, x.GenericParameterTypes[0]));
		}

		private static void RegisterFeature(IServiceCollection serviceCollection, Type featureImplementationType, Type featureStateType)
		{
			Type featureInterfaceType = typeof(IFeature<>).MakeGenericType(featureStateType);
			serviceCollection.AddScoped(featureInterfaceType, featureImplementationType);
		}
	}
}
