using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Blazor.Fluxor.DependencyInjection.DependencyScanners
{
    internal static class ReducersRegistration
    {
		internal static IEnumerable<DiscoveredReducerInfo> DiscoverReducers(IServiceCollection serviceCollection, Assembly[] assembliesToParse)
		{
			IEnumerable<DiscoveredReducerInfo> discoveredReducerInfos = assembliesToParse
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

			return discoveredReducerInfos;
		}

		private static void RegisterReducer(IServiceCollection serviceCollection, DiscoveredReducerInfo discoveredReducerInfo)
		{
			serviceCollection.AddScoped(
				serviceType: discoveredReducerInfo.ReducerInterfaceGenericType,
				implementationType: discoveredReducerInfo.ImplementingType);
		}
	}
}
