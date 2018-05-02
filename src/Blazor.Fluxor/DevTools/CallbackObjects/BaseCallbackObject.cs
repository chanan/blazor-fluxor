namespace Blazor.Fluxor.DevTools.CallbackObjects
{
	internal class BaseCallbackObject<TPayload>
		where TPayload: BasePayload
	{
#pragma warning disable IDE1006 // Naming Styles
		public string type { get; set; }
		public TPayload payload { get; set; }
#pragma warning restore IDE1006 // Naming Styles
	}

	internal class BaseCallbackObject : BaseCallbackObject<BasePayload> { }
}
