namespace Blazor.Fluxor
{
    public interface IStoreMiddleware
    {
		void Initialize(IStore store);
		void BeforeDispatch<TAction>(IStore store, TAction action)
			where TAction : IAction;
		void AfterDispatch<TAction>(IStore store, TAction action)
			where TAction : IAction;
	}
}
