using System;
using System.Threading.Tasks;

namespace Blazor.Fluxor
{
	public interface IStore
	{
		void AddEffect(Type actionType, IEffect effect);
		void AddFeature(IFeature feature);
		Task Dispatch<TAction>(TAction action) 
			where TAction : IAction;
	}
}
