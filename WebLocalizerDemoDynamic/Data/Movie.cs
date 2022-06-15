using System;

namespace WebLocalizerDemoDynamic.Data
{
    /// <summary>
    /// The movie.
    /// </summary>
    public class Movie
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the realease year.
        /// </summary>
        public DateTime RealeaseYear { get; set; }
    }
}
