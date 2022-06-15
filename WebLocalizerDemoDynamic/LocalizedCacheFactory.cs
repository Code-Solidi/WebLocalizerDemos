
using WebLocalizer.Cache;

using WebLocalizerDemoDynamic.Data;

namespace WebLocalizerDemoDynamic
{
    /// <summary>
    /// The localized cache factory.
    /// </summary>
    public class LocalizedCacheFactory : JsonStringLocalizerCacheFactory
    {
        private readonly ApplicationDbContext dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizedCacheFactory"/> class.
        /// </summary>
        /// <param name="localizedStringsProvider">The localized strings provider.</param>
        /// <param name="dbContext">The db context.</param>
        public LocalizedCacheFactory(IJsonLocalizedStringsProvider localizedStringsProvider, ApplicationDbContext dbContext)
            : base(localizedStringsProvider)
        {
            this.dbContext = dbContext;
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
            return new MovieJsonPropertyLocalizer(instance, property, id, this.dbContext);
        }
    }
}
