using Blazor.Fluxor.DependencyInjection;
using Blazor.Fluxor.DevTools;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Blazor.Fluxor
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddFluxor(this IServiceCollection serviceCollection, Action<Options> configure)
		{
			if (configure == null)
				throw new ArgumentNullException(nameof(configure));

			var options = new Options();
			configure(options);

			// Ensure ReduxToolsMiddleware is added to the middleware list
			if (options.DebugToolsEnabled)
			{
				ClientOptions.DebugToolsEnabled = true;
				ClientOptions.MiddlewareTypesList.Add(typeof(ReduxToolsMiddleware));
			}

			// Register all middleware types with dependency injection
			foreach (Type middlewareType in ClientOptions.MiddlewareTypes)
				serviceCollection.AddScoped(middlewareType);

			// Scan for features and effects
			if (options.DependencyInjectionEnabled)
				DependencyScanner.Scan(serviceCollection, options.DependencyInjectionAssembliesToScan);

			return serviceCollection;
		}
	}
}
