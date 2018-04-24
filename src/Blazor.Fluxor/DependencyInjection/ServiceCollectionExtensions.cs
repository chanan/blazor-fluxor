using Blazor.Fluxor.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Blazor.Fluxor
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddFluxor(this IServiceCollection serviceCollection, params Assembly[] assembliesToScan)
		{
			return DependencyScanner.Scan(serviceCollection, assembliesToScan);
		}
	}
}
