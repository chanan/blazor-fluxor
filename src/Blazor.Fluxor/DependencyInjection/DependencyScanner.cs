using Blazor.Fluxor.DependencyInjection;
using Blazor.Fluxor.DependencyInjection.DependencyScanners;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Blazor.Fluxor.DependencyInjection
{
	internal static class DependencyScanner
	{
		public static void Scan(this IServiceCollection serviceCollection, params Assembly[] assembliesToScan)
		{
			if (assembliesToScan == null || assembliesToScan.Length == 0)
				throw new ArgumentNullException(nameof(assembliesToScan));

			IEnumerable<DiscoveredReducerInfo> discoveredReducerInfos = ReducersRegistration.DiscoverReducers(serviceCollection, assembliesToScan);
			IEnumerable<DiscoveredEffectInfo> discoveredEffectInfos = EffectsRegistration.DiscoverEffects(serviceCollection, assembliesToScan);
			FeaturesRegistration.DiscoverFeatures(serviceCollection, assembliesToScan, discoveredReducerInfos);
			RegisterStore(serviceCollection, discoveredEffectInfos);
		}

		private static void RegisterStore(IServiceCollection serviceCollection, IEnumerable<DiscoveredEffectInfo> discoveredEffectInfos)
		{
			// Register the Store class so we can request it from the service provider
			serviceCollection.AddScoped<Store>();

			// Register a custom factory for building IStore that will inject all effects
			serviceCollection.AddScoped(typeof(IStore), serviceProvider =>
			{
				IStore store = serviceProvider.GetService<Store>();

				foreach(DiscoveredEffectInfo discoveredEffectInfo in discoveredEffectInfos)
				{
					IEffect effect = (IEffect)serviceProvider.GetService(discoveredEffectInfo.EffectInterfaceGenericType);
					store.AddEffect(discoveredEffectInfo.ActionType, effect);
				}

				foreach(Type middlewareType in ClientOptions.MiddlewareTypes)
				{
					IStoreMiddleware middleware = (IStoreMiddleware)serviceProvider.GetService(middlewareType);
					store.AddMiddleware(middleware);
				}

				return store;
			});
		}
	}
}
