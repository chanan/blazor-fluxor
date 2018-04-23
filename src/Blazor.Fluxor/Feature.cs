using System;
using System.Collections.Generic;
using System.Linq;

namespace Blazor.Fluxor
{
	public abstract class Feature<TState> : IFeature<TState>
	{
		public TState State { get; private set; }
		protected abstract TState GetInitialState();
		private readonly Dictionary<Type, List<Object>> ReducersByActionType = new Dictionary<Type, List<Object>>();

		public Feature()
		{
			State = GetInitialState();
		}

		public void AddReducer<TAction>(IReducer<TState, TAction> reducer)
		{
			AddReducer(reducer, typeof(TAction));
		}
		
		public void AddReducer(IReducer reducer, Type actionType)
		{
			if (reducer == null)
				throw new ArgumentNullException(nameof(reducer));
			if (actionType == null)
				throw new ArgumentNullException(nameof(actionType));

			if (!ReducersByActionType.TryGetValue(actionType, out List<object> reducers))
			{
				reducers = new List<object>();
				ReducersByActionType[actionType] = reducers;
			}
			reducers.Add(reducer);
		}

		public void ReceiveDispatchNotificationFromStore<TAction>(TAction action)
		{
			if (action == null)
				throw new ArgumentNullException(nameof(action));

			IEnumerable<IReducer<TState, TAction>> reducers = GetReducersForAction<TAction>();
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

		private IEnumerable<IReducer<TState, TAction>> GetReducersForAction<TAction>()
		{
			ReducersByActionType.TryGetValue(typeof(TAction), out List<object> reducers);
			var typeSafeReducers =
				reducers != null
				? reducers.Cast<IReducer<TState, TAction>>()
				: new IReducer<TState, TAction>[0];

			return typeSafeReducers;
		}

	}

}
