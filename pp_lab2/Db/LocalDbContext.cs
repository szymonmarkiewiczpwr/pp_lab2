using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace pp_lab2.Db
{
    public class LocalDbContext : DbContext
    {
        public DbSet<WeatherData> WeatherDataSet { get; set; }
        public DbSet<WeatherMainData> WeatherMainDataSet { get; set; }

        public LocalDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=weather.db");    
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WeatherData>()
                .Property(x => x.Coord).HasConversion(
                x => JsonConvert.SerializeObject(x),
                x => JsonConvert.DeserializeObject<Dictionary<string, float>>(x)
                );

            modelBuilder.Entity<WeatherData>()
                .HasOne(x => x.Main).WithOne().HasForeignKey<WeatherData>(x => x.MainId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
