using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.Fluxor
{
	public class Store : IStore
	{
		private readonly List<IFeature> Features = new List<IFeature>();
		private readonly Dictionary<Type, List<IEffect>> EffectsByActionType = new Dictionary<Type, List<IEffect>>();

		public void AddFeature(IFeature feature) => Features.Add(feature ?? throw new ArgumentNullException(nameof(feature)));

		public async Task Dispatch(IAction action)
		{
			if (action == null)
				throw new ArgumentNullException(nameof(action));

			Console.WriteLine("Dispatching action: " + action.GetType().Name);
			// Notify all features of this action
			Features.ForEach(x => x.ReceiveDispatchNotificationFromStore(action));

			// Trigger all effects registered for this action
			await TriggerEffects(action);
		}

		public void AddEffect(Type actionType, IEffect effect)
		{
			if (actionType == null)
				throw new ArgumentNullException(nameof(actionType));
			if (effect == null)
				throw new ArgumentNullException(nameof(effect));

			Type genericType = typeof(IEffect<>).MakeGenericType(actionType);
			if (!genericType.IsAssignableFrom(effect.GetType()))
				throw new ArgumentException($"Effect {effect.GetType().Name} does not implement IEffect<{actionType.Name}>");

			List<IEffect> effects = GetEffectsForActionType(actionType, true);
			effects.Add(effect);
		}

		private List<IEffect> GetEffectsForActionType(Type actionType, bool createIfNonExistent)
		{
			EffectsByActionType.TryGetValue(actionType, out List<IEffect> effects);
			if (createIfNonExistent && effects == null)
			{
				effects = new List<IEffect>();
				EffectsByActionType[actionType] = effects;
			}
			return effects;
		}

		private async Task TriggerEffects(IAction action)
		{
			IEnumerable<IEffect> effectsForAction = GetEffectsForActionType(action.GetType(), false);

			if (effectsForAction != null && effectsForAction.Any())
			{
				Console.WriteLine(effectsForAction.Count() + " effects registered");
				foreach(var effect in effectsForAction)
				{
					Console.WriteLine("Executing effect " + effect.GetType().Name);
					IAction actionFromSideEffect = await effect.Handle(action);
					Console.WriteLine("Name of action returned from effect: " + actionFromSideEffect.GetType().Name);
					if (actionFromSideEffect != null)
						await Dispatch(actionFromSideEffect);
				}
				//IEnumerable<Task<IAction>> effectTasks = effectsForAction.Select(x => x.Handle(action));
				//Console.WriteLine("About to call Task.WhenAll");
				//await Task.WhenAll(effectTasks);
				//Console.WriteLine("Task.WhenAll done. About to select the results");
				//IEnumerable<IAction> actionsFromEffectsToDispatch = effectTasks.Select(x => x.Result).Where(x => x != null);
				//Console.WriteLine($"Executing effects resulted in {actionsFromEffectsToDispatch.Count()} new actions");
				//foreach (IAction actionFromSideEffect in actionsFromEffectsToDispatch)
				//{
				//	Console.WriteLine($"Received this action from side effect: {actionFromSideEffect.GetType().Name}");
				//	await Dispatch(actionFromSideEffect);
				//}
			}
		}

	}
}
