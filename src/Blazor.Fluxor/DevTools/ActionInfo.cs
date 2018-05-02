using System;

namespace Blazor.Fluxor.DevTools
{
    internal class ActionInfo
    {
#pragma warning disable IDE1006 // Naming Styles
		public string type { get; }
#pragma warning restore IDE1006 // Naming Styles
		public object Payload { get; }

		public ActionInfo(IAction action)
		{
			if (action == null)
				throw new ArgumentNullException(nameof(action));

			type = action.GetType().FullName;
			Payload = action;
		}
    }
}
