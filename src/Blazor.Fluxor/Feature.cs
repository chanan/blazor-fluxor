using Microsoft.AspNetCore.Blazor;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blazor.Fluxor
{
	public abstract class Feature<TState> : IFeature<TState>
	{
		public abstract string GetName();
		public TState State { get; private set; }
		public object GetState() => State;
		public void RestoreState(object value) => State = (TState)value;
		public Type GetStateType() => typeof(TState);

		protected abstract TState GetInitialState();
		private readonly Dictionary<Type, List<Object>> ReducersByActionType = new Dictionary<Type, List<Object>>();

		public Feature()
		{
			State = GetInitialState();
		}

		public void AddReducer<TAction>(IReducer<TState, TAction> reducer)
		{
			if (reducer == null)
				throw new ArgumentNullException(nameof(reducer));

			Type actionType = typeof(TAction);
			if (!ReducersByActionType.TryGetValue(actionType, out List<object> reducers))
			{
				reducers = new List<object>();
				ReducersByActionType[actionType] = reducers;
			}
			reducers.Add(reducer);
		}

		public void ReceiveDispatchNotificationFromStore<TAction>(TAction action)
			where TAction: IAction
		{
			if (action == null)
				throw new ArgumentNullException(nameof(action));

			IEnumerable<IReducer<TState, TAction>> reducers = GetReducersForAction<TAction>(action);
			TState newState = State;
			if (reducers != null)
			{
				foreach (IReducer<TState, TAction> currentReducer in reducers)
				{
					newState = currentReducer.Reduce(newState, action);
				}
			}
			State = newState;
		}

		private IEnumerable<IReducer<TState, TAction>> GetReducersForAction<TAction>(IAction action)
		{
			ReducersByActionType.TryGetValue(action.GetType(), out List<object> reducers);
			var typeSafeReducers =
				reducers != null
				? reducers.Cast<IReducer<TState, TAction>>()
				: new IReducer<TState, TAction>[0];

			return typeSafeReducers;
		}

	}

}
