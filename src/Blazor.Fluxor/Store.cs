using System;
using System.Collections.Generic;

namespace Blazor.Fluxor
{
	public class Store: IStore
	{
		private readonly List<Feature> Features = new List<Feature>();

		public void Dispatch<TAction>(TAction action) => Features.ForEach(x => x.Dispatch(action));
		public void RegisterFeature(Feature feature) => Features.Add(feature ?? throw new ArgumentNullException(nameof(feature)));
	}
}
