using System;

namespace Blazor.Fluxor.DependencyInjection
{
    internal class DiscoveredFeatureInfo
    {
		public readonly Type FeatureInterfaceGenericType;
		public readonly Type ImplementingType;
		public readonly Type StateType;

		public DiscoveredFeatureInfo(Type implementingType, Type stateType)
		{
			FeatureInterfaceGenericType = typeof(IFeature<>).MakeGenericType(stateType);
			ImplementingType = implementingType;
			StateType = stateType;
		}
	}
}
