using System.Dynamic;

namespace Blazor.Fluxor.DevTools.CallbackObjects
{
	internal class JumpToStateCallback: BaseCallbackObject<JumpToStatePayload>
	{
#pragma warning disable IDE1006 // Naming Styles
		public string type { get; set; }
		public string state { get; set; }
#pragma warning restore IDE1006 // Naming Styles
	}
}
