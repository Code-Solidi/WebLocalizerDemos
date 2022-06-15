/*
 * Copyright Code Solidi Ltd. (c) 2021, 2022. All rights reserved.
 */

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace WebLocalizerRazorPagesDemo.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostSetLang()
        {
            var data = await Request.BodyReader.ReadAsync().ConfigureAwait(false);
            var lang = System.Text.Encoding.Default.GetString(data.Buffer.FirstSpan);

            try
            {
                var cultureName = CultureInfo.CurrentCulture.Name;
                switch (lang)
                {
                    case "English":
                        cultureName = "en-US";
                        break;
                    case "Italian":
                        cultureName = "it-IT";
                        break;
                    case "French":
                        cultureName = "fr-FR";
                        break;
                    case "German":
                        cultureName = "de-DE";
                        break;
                }

                var culture = new CultureInfo(cultureName);
                CultureInfo.CurrentCulture = culture;

                this.Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName
                    , CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture))
                    , new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddDays(1),
                    });

                this.Response.StatusCode = StatusCodes.Status200OK;
                return RedirectToPage("./Index");
            }
            catch
            {
                this.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return BadRequest();
            }
        }
    }
}