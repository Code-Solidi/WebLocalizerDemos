/*
 * Copyright Code Solidi Ltd. (c) 2021, 2022. All rights reserved.
 */

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using System;
using System.Linq;

using WebLocalizer.Cache;

using WebLocalizerBlazorSrvDemo.Data;

namespace WebLocalizerBlazorSrvDemo
{
    /// <summary>
    /// The weather forecast json property localizer.
    /// </summary>
    public class WeatherForecastJsonPropertyLocalizer : JsonPropertyLocalizer
    {
        private readonly string connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeatherForecastJsonPropertyLocalizer"/> class.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="property">The property.</param>
        /// <param name="id">The id.</param>
        public WeatherForecastJsonPropertyLocalizer(object instance, string property, string id, string connectionString)
            : base(instance, property, id)
        {
            // NOTE: using the injected DbContext causes concurrent (threading) issues,
            // use a new instance when saving changed property values (see Save())
            this.connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        /// <summary>
        /// Saves the localized value in the property.
        /// </summary>
        public override void Save()
        {
            if (Guid.TryParse(this.id, out var entityId))
            {
                // NOTE: we need a new instance, see comment in ctor
                var dbContext = new ApplicationDbContext(this.connectionString);

                // nb: check the instance type so as to determine the entity set type. this is an example so we already know everything!
                var weatherForecast = (WeatherForecast)dbContext.Find(typeof(WeatherForecast), entityId);
                if (weatherForecast == default)
                {
                    // something's wrong either with the entity type or with the id -- cannot find the entity
                    throw new InvalidOperationException($"Cannot map to entity of type '{nameof(WeatherForecast)}'.");
                }

                switch (this.property.Name)
                {
                    case nameof(weatherForecast.Summary):
                        weatherForecast.Summary = (string)this.property.GetValue(this.instance);
                        break;
                }

                dbContext.SaveChanges();
            }
        }
    }
}