using System;

namespace Blazor.Fluxor.DependencyInjection
{
    internal class DiscoveredReducerInfo
    {
		public readonly Type ReducerInterfaceGenericType;
		public readonly Type ImplementingType;
		public readonly Type StateType;
		public readonly Type ActionType;

		public DiscoveredReducerInfo(Type implementingType, Type stateType, Type actionType)
		{
			ReducerInterfaceGenericType = typeof(IReducer<,>).MakeGenericType(stateType, actionType);
			ImplementingType = implementingType;
			StateType = stateType;
			ActionType = actionType;
		}
	}
}
