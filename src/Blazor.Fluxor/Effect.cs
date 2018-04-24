using System.Threading.Tasks;

namespace Blazor.Fluxor
{
	public abstract class Effect<TAction> : IEffect<TAction>
	  where TAction : IAction
	{
		public abstract Task<IAction[]> Handle(TAction action);

		Task<IAction[]> IEffect.Handle(IAction action)
		{
			return Handle((TAction)action);
		}
	}
}
