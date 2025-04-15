using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pp_lab2.Db
{
    public record WeatherData
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();
        
        public Dictionary<string, float>? Coord { get; init; }
        public string Base { get; init; }
        public string MainId { get; init; }
        public WeatherMainData Main { get; init; }
    }

    public record WeatherMainData
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();
        public float Temp { get; init; }
        public float FeelsLike { get; init; }
        public float TempMin { get; init; }
        public float TempMax { get; init; }
        public float Pressure { get; init; }
        public float Humidity { get; init; }
        public float SeaLevel { get; init; }
        public float GrndLevel { get; init; }
    }
}
