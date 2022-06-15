using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace WebLocalizerBlazorSrvDemo.Data
{
    public class WeatherForecastService
    {
        private readonly ApplicationDbContext dbContext;

        public WeatherForecastService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public WeatherForecast[] GetForecast()
        {
            return this.dbContext.Set<WeatherForecast>().ToArray();
        }
    }
}
