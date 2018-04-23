using Blazor.Fluxor;
using Microsoft.AspNetCore.Blazor.Browser.Rendering;
using Microsoft.AspNetCore.Blazor.Browser.Services;

namespace CounterSample.Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new BrowserServiceProvider(services =>
            {
					services.AddFluxor(typeof(Program).Assembly);
				});

            new BrowserRenderer(serviceProvider).AddComponent<App>("app");
        }
    }
}
