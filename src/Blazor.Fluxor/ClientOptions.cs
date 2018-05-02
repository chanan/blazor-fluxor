using System;
using System.Collections.Generic;

namespace Blazor.Fluxor
{
    public static class ClientOptions
    {
		internal readonly static List<Type> MiddlewareTypesList = new List<Type>();
		public static bool DebugToolsEnabled { get; internal set; }
		public static IEnumerable<Type> MiddlewareTypes => MiddlewareTypesList;
	}
}
