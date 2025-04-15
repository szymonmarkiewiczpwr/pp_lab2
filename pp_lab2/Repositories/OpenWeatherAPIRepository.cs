using Newtonsoft.Json;
using pp_lab2.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace pp_lab2.Repositories
{
    public class OpenWeatherAPIRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiToken = "9298fe3f5188ef69ceac0b8a4cda984e";

        public OpenWeatherAPIRepository()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri($"https://api.openweathermap.org/data/2.5/");
        }

        public async Task<WeatherResponse> GetWeatherForCity(string cityName, CancellationToken cancellationToken)
        {
            Uri endpointUri = BuildWeatherApiUri("weather", new Dictionary<string, string>()
            {
                { "q", cityName }
            });
            HttpResponseMessage message = await _httpClient.GetAsync(endpointUri, cancellationToken);
            if (!message.IsSuccessStatusCode)
                throw new WeatherAPIException($"API returned {message.StatusCode} error: \n{message.ReasonPhrase}");

            string messageRawResponse = await message.Content.ReadAsStringAsync();
            WeatherResponse? weatherResponse = JsonConvert.DeserializeObject<WeatherResponse>(messageRawResponse, new JsonSerializerSettings()
            {
                MaxDepth = 3
            });

            if (weatherResponse is null)
                throw new WeatherAPIException($"Could not deserialize following body to {nameof(WeatherResponse)}: {messageRawResponse}");

            return weatherResponse;
        }

        private Uri BuildWeatherApiUri(string path, Dictionary<string, string> query)
        {
            UriBuilder builder = new UriBuilder(_httpClient.BaseAddress!);
            builder.Path += path;
            NameValueCollection queryCollection = HttpUtility.ParseQueryString(builder.Query);

            queryCollection["appid"] = _apiToken;
            foreach (KeyValuePair<string, string> queryPair in query)
            {
                queryCollection[queryPair.Key] = queryPair.Value;
            }

            builder.Query = queryCollection.ToString();

            return builder.Uri;
        }
    }

    public class WeatherAPIException : Exception
    {
        public string ApiResponse { get; private init; }

        public WeatherAPIException(string apiResponse)
        {
            ApiResponse = apiResponse;
        }
    }
}
