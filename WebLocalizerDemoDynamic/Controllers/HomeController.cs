using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using WebLocalizerDemoDynamic.Data;
using WebLocalizerDemoDynamic.Models;

namespace WebLocalizerDemoDynamic.Controllers
{
    /// <summary>
    /// The home controller.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly ApplicationDbContext dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="dbContext">The db context.</param>
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext dbContext)
        {
            this.logger = logger;
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Returns the index.html.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>An IActionResult.</returns>
        public IActionResult Index(long id = 0)
        {
            var pager = new Pager(this.dbContext.Set<Movie>().Count()) { Current = id };
            var model = new MovieListModel(this.dbContext, pager);
            return this.View(model);
        }


        /// <summary>
        /// Returns the edit.html.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>An IActionResult.</returns>
        public IActionResult Edit(Guid id)
        {
            //var pager = new Pager(this.dbContext.Set<Movie>().Count()) { Current = id };
            var model = new MovieModel(this.dbContext, id);
            return this.View(model);
        }

        /// <summary>
        /// Privacies the.
        /// </summary>
        /// <returns>An IActionResult.</returns>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Sets the lang async.
        /// </summary>
        /// <returns>A Task.</returns>
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

                //// here the data retrieved from the database should be saved in order to persist localization changes
                //this.dbContext.SaveChanges();

                return Ok();
            }
            catch
            {
                this.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return BadRequest();
            }
        }

        /// <summary>
        /// Errors the.
        /// </summary>
        /// <returns>An IActionResult.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
