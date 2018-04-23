using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Blazor.Fluxor.DependencyInjection.DependencyScanners
{
    internal class EffectsRegistration
    {
		internal static IEnumerable<DiscoveredEffectInfo> DiscoverEffects(IServiceCollection serviceCollection, Assembly[] assembliesToScan)
		{
			IEnumerable<DiscoveredEffectInfo> discoveredEffectInfos = assembliesToScan
				.SelectMany(asm => asm.GetTypes())
				.Select(t => new
				{
					ImplementingType = t,
					GenericParameterTypes = TypeHelper.GetGenericParametersForImplementedInterface(t, typeof(IEffect<>))
				})
				.Where(x => x.GenericParameterTypes != null)
				.Select(x => new DiscoveredEffectInfo(
					implementingType: x.ImplementingType,
					actionType: x.GenericParameterTypes[0]
					)
				)
				.ToList();

			foreach (DiscoveredEffectInfo discoveredEffectInfo in discoveredEffectInfos)
			{
				RegisterEffect(serviceCollection, discoveredEffectInfo);
			}

			return discoveredEffectInfos;
		}

		private static void RegisterEffect(IServiceCollection serviceCollection, DiscoveredEffectInfo discoveredEffectInfo)
		{
			serviceCollection.AddScoped(
				serviceType: discoveredEffectInfo.EffectInterfaceGenericType,
				implementationType: discoveredEffectInfo.ImplementingType);
		}
	}
}
