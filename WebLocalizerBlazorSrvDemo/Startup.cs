using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

using WebLocalizer.Cache;
using WebLocalizer.Common;

using WebLocalizerBlazorSrvDemo.Components;
using WebLocalizerBlazorSrvDemo.Data;

namespace WebLocalizerBlazorSrvDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                var connectionString = this.Configuration.GetConnectionString("DefaultConnection");
                options.UseSqlServer(connectionString);
            });

            services.AddScoped<WeatherForecastService>();

            #region Localization

            var supportedCultures = new[]
            {
                new CultureInfo("en"),
                new CultureInfo("it"),
                new CultureInfo("fr"),
                new CultureInfo("de")
            };

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("en", "en");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.RequestCultureProviders = new List<IRequestCultureProvider>
                {
                    new CookieRequestCultureProvider()
                };
            });

            services.AddLocalization();

            // NB: required by both LanguageSelector and WebLocalizer components (look into Components folder)
            services.AddScoped<INotifierService, NotifierService>();

            #endregion Localization

            #region WebLocalizer

            services.AddWebLocalizer(options =>
            {
                this.Configuration.GetSection("WebLocalizer").Bind(options);
                options.DefaultLanguage = "en";
            });

            services.AddScoped<JsonStringLocalizerCacheFactory, LocalizedCacheFactory>();

            #endregion WebLocalizer
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            #region Localization

            var supportedCultures = new[] { "en", "it", "fr", "de" };
            var localizationOptions = new RequestLocalizationOptions()
                .SetDefaultCulture(supportedCultures[0])
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);
            app.UseRequestLocalization(localizationOptions);

            #endregion Localization

            #region WebLocalizer

            app.UseWebLocalizer();

            #endregion WebLocalizer

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });


            this.PopulateDb(dbContext);
        }

        /// <summary>
        /// Populates the db.
        /// </summary>
        /// <param name="dbContext">The db context.</param>
        private void PopulateDb(ApplicationDbContext dbContext)
        {
            dbContext.Database.Migrate();
            var wetearForecastSet = dbContext.Set<WeatherForecast>();
            if (wetearForecastSet.CountAsync().Result == 0)
            {
                string[] summaries = new[]
                {
                    "This month will be mostly Cloudy. The average daily high/low will be 23°C/9°C. The expected highest/lowest temperature is 28°C/2°C. There will be 4 rainy day.",
                    "This month will be mostly Sunny. The average daily high/low will be 28°C/16°C. The expected highest/lowest temperature is 31°C/14°C.",
                    "This month will be mostly Cloudy. The average daily high/low will be 31°C/18°C. The expected highest/lowest temperature is 32°C/16°C.",
                    "This month will be mostly Cloudy. The average daily high/low will be 26°C/14°C. The expected highest/lowest temperature is 28°C/12°C.",
                    "This month will be mostly Cloudy. The average daily high/low will be 13°C/4°C. The expected highest/lowest temperature is 17°C/1°C. There will be 1 snowy day.",
                    "This month will be mostly Cloudy. The average daily high/low will be 7°C/-1°C. The expected highest/lowest temperature is 10°C/-4°C.",
                    "This month will be mostly Cloudy. The average daily high/low will be 6°C/-2°C. The expected highest/lowest temperature is 7°C/-3°C. There will be 6 snowy day.",
                    "This month will be mostly Cloudy. The average daily high/low will be 8°C/-1°C. The expected highest/lowest temperature is 11°C/-2°C. There will be 2 snowy day.",
                    "This month will be mostly Sunny. The average daily high/low will be 13°C/3°C. The expected highest/lowest temperature is 15°C/1°C.",
                    "This month will be mostly Sunny. The average daily high/low will be 19°C/8°C. The expected highest/lowest temperature is 22°C/4°C."
                };

                var random = new Random();
                var date = DateTime.Now.Date;
                for (var index = 0; index < summaries.Length; index++) 
                {
                    var forecast = new WeatherForecast
                    {
                        Date = date.AddDays(index),
                        TemperatureC = random.Next(-20, 55),
                        Summary = summaries[index]
                    };

                    wetearForecastSet.Add(forecast);
                }

                dbContext.SaveChanges();
            }
        }
    }
}
