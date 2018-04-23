using System;

namespace Blazor.Fluxor.DependencyInjection
{
    internal class DiscoveredReducerInfo
    {
		public readonly Type StateType;
		public readonly Type ActionType;

		public DiscoveredReducerInfo(Type stateType, Type actionType)
		{
			StateType = stateType;
			ActionType = actionType;
		}
	}
}
