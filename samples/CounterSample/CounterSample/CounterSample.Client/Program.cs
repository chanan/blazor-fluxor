using Blazor.Fluxor;
using CounterSample.Client.Store.Counter;
using Microsoft.AspNetCore.Blazor.Browser.Rendering;
using Microsoft.AspNetCore.Blazor.Browser.Services;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CounterSample.Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new BrowserServiceProvider(services =>
            {
					services.AddSingleton<IStore, Blazor.Fluxor.Store>();
					services.AddSingleton<IFeature<CounterState>, CounterFeature>();
				});

            new BrowserRenderer(serviceProvider).AddComponent<App>("app");
        }
    }
}
