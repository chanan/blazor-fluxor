using System;

namespace Blazor.Fluxor
{
	public interface IStore
	{
		void Dispatch<TAction>(TAction action);
		void RegisterFeature(Feature feature);
	}
}
