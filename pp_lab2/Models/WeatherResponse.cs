using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pp_lab2.Models
{
    public record WeatherResponse
    {
        public Dictionary<string, float>? Coord { get; init; }
        public string Base { get; init; }
        public WeatherMainResponse Main { get; init; }
    }

    public record WeatherMainResponse
    {
        public float Temp { get; init; }
        [JsonProperty("feels_like")]
        public float FeelsLike { get; init; }
        [JsonProperty("temp_min")]
        public float TempMin { get; init; }
        [JsonProperty("temp_max")]
        public float TempMax { get; init; }
        public float Pressure { get; init; }
        public float Humidity { get; init; }
        [JsonProperty("sea_level")]
        public float SeaLevel { get; init; }
        [JsonProperty("grnd_level")]
        public float GrndLevel { get; init; }
    }
}
