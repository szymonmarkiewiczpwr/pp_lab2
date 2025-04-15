using Mapster;
using Microsoft.EntityFrameworkCore;
using pp_lab2.Db;
using pp_lab2.Models;
using pp_lab2.Repositories;

namespace pp_lab2
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            LocalDbContext localDbContext = new LocalDbContext();

            CancellationToken cancellationToken = new CancellationToken();
            OpenWeatherAPIRepository repository = new OpenWeatherAPIRepository();
            WeatherResponse city = await repository.GetWeatherForCity("warsaw", cancellationToken);
            WeatherData cityDb = city.Adapt<WeatherData>();

            localDbContext.Add(cityDb);
            await localDbContext.SaveChangesAsync();

            List<WeatherData> dbWeatherData = await localDbContext.WeatherDataSet.Include(x => x.Main).ToListAsync();
            foreach (WeatherData data in dbWeatherData)
            {
                Console.WriteLine(data.ToString());
            }

            localDbContext.Remove(dbWeatherData.First());
            await localDbContext.SaveChangesAsync();
        }
    }
}
