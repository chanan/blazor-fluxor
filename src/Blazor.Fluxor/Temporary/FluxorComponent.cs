using Blazor.Fluxor.DevTools;
using Microsoft.AspNetCore.Blazor.Components;
using System;

namespace Blazor.Fluxor.Temporary
{
	// This class is temporary until Blazor has some kind of global StateHasChanged() call available
	// See https://github.com/aspnet/Blazor/issues/704
	public class FluxorComponent : BlazorComponent, IDisposable
	{
		public FluxorComponent()
		{
			ReduxDevTools.AfterJumpToState += ReduxDevTools_AfterJumpToState;
		}

		public void Dispose()
		{
			ReduxDevTools.AfterJumpToState -= ReduxDevTools_AfterJumpToState;
		}

		private void ReduxDevTools_AfterJumpToState(object sender, EventArgs e)
		{
			StateHasChanged();
		}

	}
}
