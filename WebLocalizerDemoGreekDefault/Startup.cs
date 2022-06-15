/*
 * Copyright Code Solidi Ltd. (c) 2021, 2022. All rights reserved.
 */

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System.Collections.Generic;
using System.Globalization;

using WebLocalizer.Common;

namespace WebLocalizerDemo
{
    /// <summary>
    /// The startup.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="services">The services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(this.Configuration);
            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            #region Localization

            var supportedCultures = new[]
            {
                new CultureInfo("el"),
                new CultureInfo("it"),
                new CultureInfo("fr"),
                new CultureInfo("de")
            };

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("el", "el");
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
                options.DefaultLanguage = "el";
            });

            #endregion WebLocalizer
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// Configures this.
        /// </summary>
        /// <param name="app">The app.</param>
        /// <param name="env">The env.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            #region Localization

            var supportedCultures = new[] { "el", "it", "fr", "de" };
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
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}