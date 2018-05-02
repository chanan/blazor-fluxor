using System;

namespace ReduxDevToolsIntegration.Client.Store.Counter
{
	// TODO: Make property setters private - https://github.com/aspnet/Blazor/issues/705
	public class CounterState
	{
		public int Value { get; set; }

		[Obsolete("For deserialization purposes only. Use the constructor with parameters")]
		public CounterState() { } // Required by DevTools to recreate historic state

		public CounterState(int value)
		{
			Value = value;
		}
	}
}
