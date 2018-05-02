using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Blazor.Fluxor.DependencyInjection
{
	public class Options
	{

		internal bool DebugToolsEnabled { get; private set; }
		internal bool DependencyInjectionEnabled { get; private set; }
		internal Assembly[] DependencyInjectionAssembliesToScan { get; private set; } = new Assembly[0];
		internal Type[] MiddlewareTypes { get; private set; } = new Type[0];

		public Options UseDebugTools()
		{
			DebugToolsEnabled = true;
			return this;
		}

		public Options UseDependencyInjection(params Assembly[] assembliesToScan)
		{
			if (assembliesToScan == null || assembliesToScan.Length == 0)
				throw new ArgumentNullException(nameof(assembliesToScan));

			DependencyInjectionEnabled = true;
			DependencyInjectionAssembliesToScan = DependencyInjectionAssembliesToScan
				.Union(assembliesToScan)
				.Distinct()
				.ToArray();

			return this;
		}

		public Options AddMiddleware<TMiddleware>()
			where TMiddleware : IStoreMiddleware
		{
			MiddlewareTypes = new List<Type>(MiddlewareTypes)
			{
				typeof(TMiddleware)
			}
			.Distinct()
			.ToArray();

			return this;
		}
	}
}
