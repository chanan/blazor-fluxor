using System;

namespace Blazor.Fluxor
{
	public interface IStore
	{
		void AddFeature(IFeature feature);
		void Dispatch<TAction>(TAction action);
	}
}
