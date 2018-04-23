using System;
using System.Collections.Generic;

namespace Blazor.Fluxor
{
	public class Store: IStore
	{
		private readonly List<IFeature> Features = new List<IFeature>();

		public void Dispatch<TAction>(TAction action) => Features.ForEach(x => x.ReceiveDispatchNotificationFromStore(action));
		public void AddFeature(IFeature feature) => Features.Add(feature ?? throw new ArgumentNullException(nameof(feature)));
	}
}
