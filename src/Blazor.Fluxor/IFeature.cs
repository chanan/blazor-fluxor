using System;

namespace Blazor.Fluxor
{
	public interface IFeature
	{
		/// <summary>
		/// The unique name to use for this feature when building up the composite state. E.g. if this returns "Cart" then
		/// the composite state returned would contain a property "Cart" with a value that represents the contents of State.
		/// </summary>
		/// <returns></returns>
		string GetName();
		object GetState();
		Type GetStateType();
		void RestoreState(object value);
		void ReceiveDispatchNotificationFromStore<TAction>(TAction action)
			where TAction : IAction;
	}

	public interface IFeature<TState>: IFeature
	{
		TState State { get; }
		void AddReducer<TAction>(IReducer<TState, TAction> reducer);
	}
}
