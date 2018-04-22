using System;
using System.Collections.Generic;
using System.Linq;

namespace Blazor.Fluxor
{
	public abstract class Feature
	{
		internal abstract void Dispatch<TAction>(TAction action);
	}

	public abstract class Feature<TState> : Feature, IFeature<TState>
	{
		private readonly Dictionary<Type, List<Object>> ReducersByActionType = new Dictionary<Type, List<Object>>();

		protected abstract void RegisterReducers();
		protected abstract TState GetInitialState();

		public TState State { get; private set; }

		public Feature(IStore store)
		{
			store.RegisterFeature(this);
			RegisterReducers();
			State = GetInitialState();
		}

		internal override void Dispatch<TAction>(TAction action)
		{
			if (action == null)
				throw new ArgumentNullException(nameof(action));

			IEnumerable<IReducer<TState, TAction>> reducers = GetReducersForAction<TAction>();
			TState newState = State;
			if (reducers != null)
			{
				foreach (IReducer<TState, TAction> currentReducer in reducers)
				{
					newState = currentReducer.Reduce(action, newState);
				}
			}
			State = newState;
		}

		protected void RegisterReducer<TAction>(IReducer<TState, TAction> reducer)
		{
			if (reducer == null)
				throw new ArgumentNullException(nameof(reducer));

			if (!ReducersByActionType.TryGetValue(typeof(TAction), out List<object> reducers))
			{
				reducers = new List<object>();
				ReducersByActionType[typeof(TAction)] = reducers;
			}
			reducers.Add(reducer);
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
