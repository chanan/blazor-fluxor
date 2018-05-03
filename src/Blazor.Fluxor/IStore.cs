using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blazor.Fluxor
{
	public interface IStore
	{
		void AddEffect(Type actionType, IEffect effect);
		void AddFeature(IFeature feature);
		void AddMiddleware(IStoreMiddleware middleware);
		Task DispatchAsync<TAction>(TAction action)
			where TAction : IAction;
		IEnumerable<IFeature> Features { get; }
	}
}
