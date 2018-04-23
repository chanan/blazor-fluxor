using System.Threading.Tasks;

namespace Blazor.Fluxor
{
	public interface IEffect
	{
		Task<IAction> Handle(IAction action);
	}

	public interface IEffect<TAction>: IEffect
	  where TAction : IAction
	{
		Task<IAction> Handle(TAction action);
	}
}
