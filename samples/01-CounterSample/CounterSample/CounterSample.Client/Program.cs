using Microsoft.AspNetCore.Blazor.Browser.Rendering;
using Microsoft.AspNetCore.Blazor.Browser.Services;
using Blazor.Fluxor;

namespace CounterSample.Client
{
	public class Program
	{
		static void Main(string[] args)
		{
			var serviceProvider = new BrowserServiceProvider(services =>
			{
				services.AddFluxor(options => options
					.UseDependencyInjection(typeof(Program).Assembly)
				);
			});

			new BrowserRenderer(serviceProvider).AddComponent<App>("app");
		}
	}
}
