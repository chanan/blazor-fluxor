using Blazor.Fluxor.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Blazor.Fluxor
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddFluxor(this IServiceCollection serviceCollection, params Assembly[] assembliesToParse)
		{
			return DependencyScanner.Scan(serviceCollection, assembliesToParse);
		}
	}
}
