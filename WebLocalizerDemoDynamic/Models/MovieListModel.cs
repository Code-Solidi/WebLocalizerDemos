/*
 * Copyright Code Solidi Ltd. (c) 2021, 2022. All rights reserved.
 */

using System.Collections.Generic;
using System.Linq;

using WebLocalizerDemoDynamic.Data;

namespace WebLocalizerDemoDynamic.Models
{
    public class MovieListModel
    {
        private readonly ApplicationDbContext dbContext;

        public MovieListModel(ApplicationDbContext dbContext, Pager pager)
        {
            this.Pager = pager;
            this.dbContext = dbContext;
            this.Movies = this.dbContext.Set<Movie>()
                .Skip((int)(pager.Current - 1) * pager.Range)
                .Take(pager.Range)
                .Select(x => new MovieModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    ReleaseYear = x.RealeaseYear
                })
                /* NB: this "materializes" the query, otherwise there might be problems
                 * when saving the localized contents to the property cache.
                 */
                .ToArray();
        }

        public IEnumerable<MovieModel> Movies { get; set; }

        public Pager Pager { get; }
    }

    public class Pager
    {
        private const int MaxSize = 11;

        public Pager(long recordCount, int size = Pager.MaxSize)
        {
            this.Range = size - 1;
            this.PageCount = recordCount / this.Range + (recordCount % this.Range != 0 ? 1 : 0);
            if (this.PageCount < this.Range)
            {
                this.Range = (int)this.PageCount;
            }
        }

        private long current = 1;

        public int Range { get; }

        public long PageCount { get; }

        public long Current
        {
            get => this.current;

            set
            {
                if (value < 1) { value = 1; }
                this.current = value == -1 || value > this.PageCount ? this.PageCount : value;

                this.Min = this.Confine(this.current - this.Range / 2, this.Min, this.PageCount - this.Range);
                this.Min = this.Confine(1, this.Min, this.PageCount - this.Range);

                this.Max = this.Min + this.Range;
                this.Max = this.Confine(this.Min + this.Range, this.Max, this.PageCount);
            }
        }

        public long Min { get; private set; }

        public long Max { get; private set; }

        public long Prev => this.Current - 1 > 1 ? this.Current - 1 : 1;

        public long Next => this.Current + 1 < this.PageCount ? this.Current + 1 : this.PageCount;

        private long Confine(long min, long curr, long max)
        {
            if (max < min) { max = min; }
            if (curr < min) { curr = min; }
            if (curr > max) { curr = max; }
            return curr;
        }
    }
}