using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

using WebLocalizerDemo.Models;

namespace WebLocalizerDemo.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SetLangAsync()
        {
            var data = await Request.BodyReader.ReadAsync().ConfigureAwait(false);
            var lang = System.Text.Encoding.Default.GetString(data.Buffer.FirstSpan);

            try
            {
                var cultureName = CultureInfo.CurrentCulture.Name;
                switch (lang)
                {
                    case "Greek":
                        cultureName = "el-GR";
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

                Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName
                    , CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture))
                    , new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddDays(1),
                    });

                Response.StatusCode = StatusCodes.Status200OK;
                return Ok();
            }
            catch
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return BadRequest();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
