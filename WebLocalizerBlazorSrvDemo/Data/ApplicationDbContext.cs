using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using System;

namespace WebLocalizerBlazorSrvDemo.Data
{
    /// <summary>
    /// The application db context.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        private string connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public ApplicationDbContext(string connectionString) => this.connectionString = connectionString;

        /// <inheritdoc/>>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(this.connectionString);
            }
        }

        /// <inheritdoc/>>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WeatherForecast>().Property(x => x.Id).IsRequired().ValueGeneratedOnAdd();
            modelBuilder.Entity<WeatherForecast>().HasKey(x => x.Id);

            modelBuilder.Entity<WeatherForecast>().Property(x => x.Date).IsRequired();
            modelBuilder.Entity<WeatherForecast>().Property(x => x.Summary).IsRequired();
            modelBuilder.Entity<WeatherForecast>().Property(x => x.TemperatureC).IsRequired();
        }
    }
}
