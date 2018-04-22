using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace Blazor.Fluxor
{
    public static class ServiceCollectionExtensions
    {
		public static IServiceCollection AddFluxor(this IServiceCollection serviceCollection, params Assembly[] assembliesToParse)
		{
			if (assembliesToParse == null || assembliesToParse.Length == 0)
				throw new ArgumentNullException(nameof(assembliesToParse));

			RegisterStore(serviceCollection);
			RegisterFeatures(serviceCollection, assembliesToParse);
			return serviceCollection;
		}

		private static void RegisterStore(IServiceCollection serviceCollection)
		{
			serviceCollection.AddScoped<IStore, Store>();
		}

		private static void RegisterFeatures(IServiceCollection serviceCollection, Assembly[] assembliesToParse)
		{
			var features = assembliesToParse
				.SelectMany(asm => asm.GetTypes())
				.Select(t => new
				{
					FeatureType = t,
					GenericParameterTypes = TypeExtensions.GetGenericParametersForSpecificGenericType(t, typeof(Feature<>))
				})
				.Where(x => x.GenericParameterTypes != null)
				.ToList();
			features.ForEach(x => RegisterFeature(serviceCollection, x.FeatureType, x.GenericParameterTypes[0]));
		}

		private static void RegisterFeature(IServiceCollection serviceCollection, Type featureImplementationType, Type featureStateType)
		{
			Console.WriteLine("FeatureType = " + featureImplementationType.FullName);
			Console.WriteLine("StateType = " + featureStateType.Name);
			Type featureInterfaceType = typeof(IFeature<>).MakeGenericType(featureStateType);
			serviceCollection.AddScoped(featureInterfaceType, featureImplementationType);
		}
	}
}
