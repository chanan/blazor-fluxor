using Blazor.Fluxor.DependencyInjection;
using Blazor.Fluxor.DependencyInjection.DependencyScanners;
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

			IEnumerable<DiscoveredReducerInfo> discoveredReducerInfos = ReducersRegistration.DiscoverReducers(serviceCollection, assembliesToParse);
			FeaturesRegistration.DiscoverFeatures(serviceCollection, assembliesToParse, discoveredReducerInfos);
			RegisterStore(serviceCollection);
			return serviceCollection;
		}

		private static void RegisterStore(IServiceCollection serviceCollection)
		{
			serviceCollection.AddScoped<IStore, Store>();
		}




	}
}
