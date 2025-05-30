﻿using DevExpress.Blazor.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace McHealthCare.Web.Components.Layout.ThemeSwitcher
{
    public class ThemeJsChangeDispatcher : ComponentBase, IThemeChangeRequestDispatcher, IAsyncDisposable
    {
        [Parameter]
        public string InitialThemeName { get; set; }
        [Inject]
        private ISafeJSRuntime JsRuntime { get; set; }
        [Inject]
        private ThemeService Themes { get; set; }

        private Theme _pendingTheme;
        private IJSObjectReference _module;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Themes.ThemeChangeRequestDispatcher = this;
            if (Themes.ActiveTheme == null)
                Themes.SetActiveThemeByName(InitialThemeName);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                await base.OnAfterRenderAsync(firstRender);
                if (firstRender)
                    _module = await JsRuntime.InvokeAsync<IJSObjectReference>("import", "./themes/theme-controller.js");
            }
            catch { }
        }

        public async void RequestThemeChange(Theme theme)
        {
            if (_pendingTheme == theme) return;
            _pendingTheme = theme;
            await _module.InvokeVoidAsync("ThemeController.setStylesheetLinks",
                Themes.GetBootstrapThemeCssUrl(theme),
                theme.BootstrapThemeMode,
                Themes.GetThemeCssUrl(theme),
                Themes.GetHighlightJSThemeCssUrl(theme),
                DotNetObjectReference.Create(this));
        }

        [JSInvokable]
        public async Task ThemeLoadedAsync()
        {
            if (Themes.ThemeLoadNotifier != null)
            {
                await Themes.ThemeLoadNotifier.NotifyThemeLoadedAsync(_pendingTheme);
            }
            _pendingTheme = null;
        }

        public async ValueTask DisposeAsync()
        {
            if (_module != null)
                await _module.DisposeAsync();
        }
    }
}
