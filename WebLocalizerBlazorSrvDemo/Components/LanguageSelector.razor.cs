/*
 * Copyright Code Solidi Ltd. (c) 2021, 2022. All rights reserved.
 */

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Localization;
using Microsoft.JSInterop;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace WebLocalizerBlazorSrvDemo.Components
{
    public partial class LanguageSelector : IAsyncDisposable
    {
        private IJSObjectReference jsModule;

        protected Dictionary<string, string> Cultures = new Dictionary<string, string>
        {
            { "en", "English" },
            { "it", "Italian" },
            { "fr", "French" },
            { "de", "German" }
        };

        [Inject]
        protected IJSRuntime jsRuntime { get; set; }

        [Inject]
        protected INotifierService Notifier { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                this.jsModule ??= await this.jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/WebLocalizer.Blazor/cookie.js");
            }
        }

        protected async Task OnSelected(ChangeEventArgs e)
        {
            if (this.jsModule != default)
            {
                var name = CookieRequestCultureProvider.DefaultCookieName;
                var culture = CookieRequestCultureProvider.MakeCookieValue(new RequestCulture((string)e.Value));

                await this.jsModule.InvokeVoidAsync("writeCookie", name, culture, 1);

                // a short-cut: instead of making a request thus letting the localization middleware to handle 
                // the culture change we do it here explicitly
                //this.UriHelper.NavigateTo(this.UriHelper.Uri, forceLoad: true);
                CultureInfo.CurrentCulture = CultureInfo.CurrentUICulture = new CultureInfo((string)e.Value);
                await this.Notifier.NotifyAsync(culture);
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (this.jsModule != null)
            {
                await this.jsModule.DisposeAsync();
                this.jsModule = null;
            }
        }
    }
}