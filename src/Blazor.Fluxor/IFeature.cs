using System;

namespace Blazor.Fluxor
{
	public interface IFeature
	{
		void AddReducer(IReducer reducer, Type actionType);
		void ReceiveDispatchNotificationFromStore<TAction>(TAction action);
	}

	public interface IFeature<TState>: IFeature
	{
		TState State { get; }
		void AddReducer<TAction>(IReducer<TState, TAction> reducer);
	}
}
