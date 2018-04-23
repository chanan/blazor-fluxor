using System;

namespace Blazor.Fluxor
{
	public interface IFeature
	{
		void RegisterReducer(IReducer reducer, Type actionType);
		void ReceiveDispatchNotificationFromStore<TAction>(TAction action);
	}

	public interface IFeature<TState>: IFeature
	{
		TState State { get; }
		void RegisterReducer<TAction>(IReducer<TState, TAction> reducer);
	}
}
