/*
 * Copyright Code Solidi Ltd. (c) 2021, 2022. All rights reserved.
 */

using Microsoft.Extensions.Configuration;

using System;

using WebLocalizer.Cache;

using WebLocalizerBlazorSrvDemo.Data;

namespace WebLocalizerBlazorSrvDemo
{
    /// <summary>
    /// The localized cache factory.
    /// </summary>
    public class LocalizedCacheFactory : JsonStringLocalizerCacheFactory
    {
        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizedCacheFactory"/> class.
        /// </summary>
        /// <param name="localizedStringsProvider">The localized strings provider.</param>
        public LocalizedCacheFactory(IJsonLocalizedStringsProvider localizedStringsProvider, IConfiguration configuration)
            : base(localizedStringsProvider)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Creates the dynamic cache.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="property">The property.</param>
        /// <param name="id">The id.</param>
        /// <returns>An IJsonPropertyLocalizer.</returns>
        public override IJsonPropertyLocalizer CreateDynamicCache(object instance, string property, string id)
        {
            return new WeatherForecastJsonPropertyLocalizer(instance, property, id, this.configuration.GetConnectionString("DefaultConnection"));
        }
    }
}