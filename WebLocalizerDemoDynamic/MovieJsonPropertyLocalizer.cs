/*
 * Copyright Code Solidi Ltd. (c) 2021. All rights reserved.
 */

using System;

using WebLocalizer.Cache;

using WebLocalizerDemoDynamic.Data;

namespace WebLocalizerDemoDynamic
{
    /// <summary>
    /// The movie json property localizer.
    /// </summary>
    public class MovieJsonPropertyLocalizer : JsonPropertyLocalizer
    {
        private readonly ApplicationDbContext dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="MovieJsonPropertyLocalizer"/> class.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="property">The property.</param>
        /// <param name="id">The id.</param>
        /// <param name="dbContext">The db context.</param>
        public MovieJsonPropertyLocalizer(object instance, string property, string id, ApplicationDbContext dbContext)
            : base(instance, property, id)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Saves the localized value in the property.
        /// </summary>
        public override void Save()
        {
            if (Guid.TryParse(this.id, out var entityId))
            {
                // NB: we have the view model type specified in the view, so we need to map it to the entity type here...
                //...

                // NB: check the instance type so as to determine the entity set type. this is an example so we already know everything!
                var movie = (Movie)this.dbContext.Find(typeof(Movie), entityId);
                if (movie == default)
                {
                    // something's wrong either with the entity type or with the id -- cannot find the entity
                    throw new InvalidOperationException($"Cannot map to entity of type '{nameof(Movie)}'.");
                }

                switch (this.property.Name)
                {
                    case nameof(movie.Description):
                        movie.Description = (string)this.property.GetValue(this.instance);
                        break;
                }

                this.dbContext.SaveChanges();
            }
        }
    }
}