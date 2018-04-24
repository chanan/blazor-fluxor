using System;

namespace Blazor.Fluxor.DependencyInjection
{
    internal class DiscoveredEffectInfo
    {
		public readonly Type EffectInterfaceGenericType;
		public readonly Type ImplementingType;
		public readonly Type ActionType;

		public DiscoveredEffectInfo(Type implementingType, Type actionType)
		{
			EffectInterfaceGenericType = typeof(IEffect<>).MakeGenericType(actionType);
			ImplementingType = implementingType;
			ActionType = actionType;
		}
	}
}
