using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

using System.IO;

namespace WebLocalizerDemoDynamic.Data
{
    /// <summary>
    /// The application db context.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Ons the model creating.
        /// </summary>
        /// <param name="modelBuilder">The model configuration.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>().Property(x => x.Id).IsRequired().ValueGeneratedOnAdd();
            modelBuilder.Entity<Movie>().HasKey(x => x.Id);

            modelBuilder.Entity<Movie>().Property(x => x.Title).IsRequired().HasMaxLength(150);
            modelBuilder.Entity<Movie>().Property(x => x.Description).IsRequired();
        }

        public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
        {
            public ApplicationDbContext CreateDbContext(string[] args)
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .AddJsonFile("appsettings.Development.json");

                var connectionString = configuration.Build().GetConnectionString("DefaultConnection");

                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionsBuilder.UseSqlServer(connectionString);
                return new ApplicationDbContext(optionsBuilder.Options);
            }
        }
    }
}
