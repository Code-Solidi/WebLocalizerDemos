/*
 * Copyright Code Solidi Ltd. (c) 2021, 2022. All rights reserved.
 */

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using WebLocalizerDemoDynamic.Data;

namespace WebLocalizerDemoDynamic.Models
{
    public class MovieModel
    {
        public MovieModel()
        {
        }

        public MovieModel(ApplicationDbContext dbContext, Guid id)
        {
            var entity = dbContext.Set<Movie>().SingleOrDefault(x => x.Id == id);
            if (entity != default)
            {
                this.Id = entity.Id;
                this.Title = entity.Title;
                this.Description = entity.Description;
                this.ReleaseYear = entity.RealeaseYear;
            }
        }

        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        [Display(Name = "Released")]
        public DateTime ReleaseYear { get; set; }
    }
}