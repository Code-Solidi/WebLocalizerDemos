using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

using WebLocalizer.Common;

namespace WebLocalizerRazorPagesDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // AFT not needed for this demo.
            services.AddRazorPages(o => o.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute()));

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

            #endregion Localization

            #region WebLocalizer

            services.AddWebLocalizer(options =>
            {
                this.Configuration.GetSection("WebLocalizer").Bind(options);
                options.DefaultLanguage = "en";
            });

            #endregion WebLocalizer
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            app.UseAuthorization();

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
                endpoints.MapRazorPages();
            });
        }
    }
}
