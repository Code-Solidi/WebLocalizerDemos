using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

using WebLocalizer.Cache;
using WebLocalizer.Common;

using WebLocalizerDemoDynamic.Data;

namespace WebLocalizerDemoDynamic
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

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                var connectionString = this.Configuration.GetConnectionString("DefaultConnection");
                options.UseSqlServer(connectionString);
            });

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

            services.AddScoped<JsonStringLocalizerCacheFactory, LocalizedCacheFactory>();

            #endregion WebLocalizer
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// Configures the.
        /// </summary>
        /// <param name="app">The app.</param>
        /// <param name="env">The env.</param>
        /// <param name="dbContext">The db context.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContext dbContext)
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
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
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
            var movieSet = dbContext.Set<Movie>();
            if (movieSet.CountAsync().Result == 0)
            {
                var index = Environment.CurrentDirectory.IndexOf(@"bin\debug", StringComparison.OrdinalIgnoreCase);
                var cwd = Environment.CurrentDirectory.Remove(index == -1 ? 0 : index);
                var json = File.ReadAllText(Path.Combine(cwd, "movies.json"));

                var options = new JsonSerializerOptions();
                options.Converters.Add(new CustomDateTimeConverter());
                foreach (var movie in (Movie[])JsonSerializer.Deserialize(json, typeof(Movie[]), options))
                {
                    movieSet.Add(movie);
                }

                dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// The custom date time converter.
        /// </summary>
        public class CustomDateTimeConverter : JsonConverter<DateTime>
        {
            /// <summary>
            /// Reads the.
            /// </summary>
            /// <param name="reader">The reader.</param>
            /// <param name="typeToConvert">The type to convert.</param>
            /// <param name="options">The options.</param>
            /// <returns>A DateTime.</returns>
            public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return DateTime.ParseExact(reader.GetString(), "yyyy", null);
            }

            /// <summary>
            /// Writes the.
            /// </summary>
            /// <param name="writer">The writer.</param>
            /// <param name="value">The value.</param>
            /// <param name="options">The options.</param>
            public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
            {                //Don't implement this unless you're going to use the custom converter for serialization too
                throw new NotImplementedException();
            }
        }
    }
}