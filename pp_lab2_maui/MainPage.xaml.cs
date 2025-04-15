using Mapster;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using pp_lab2.Db;
using pp_lab2.Models;
using pp_lab2.Repositories;
using System.Collections.ObjectModel;

namespace pp_lab2_maui
{
    public partial class MainPage : ContentPage
    {
        private readonly LocalDbContext _localDbContext = new LocalDbContext();
        private readonly OpenWeatherAPIRepository _weatherRepository = new OpenWeatherAPIRepository();

        private ObservableCollection<WeatherData> _weatherDataList = new ObservableCollection<WeatherData>();

        private event EventHandler _weatherDataListChanged;

        public MainPage()
        {
            InitializeComponent();

            weatherDataListView.ItemsSource = _weatherDataList;

            _weatherDataListChanged += OnWeatherDataListChanged;
            _weatherDataListChanged?.Invoke(this, EventArgs.Empty);
        }

        private void OnWeatherDataListChanged(object? sender, EventArgs e)
        {
            var list = _localDbContext.WeatherDataSet.Include(x => x.Main).ToList();
            _weatherDataList.Clear();
            list.ForEach(x => _weatherDataList.Add(x));
        }

        private void OnAddWeatherDataClicked(object sender, EventArgs e)
        {
            Task.Run(async () =>
            {
                await _localDbContext.AddAsync(new WeatherData
                {
                    Id = Guid.NewGuid().ToString(),
                    Coord = JsonConvert.DeserializeObject<Dictionary<string, float>>(coordEntry.Text),
                    Main = new WeatherMainData
                    {
                        Id = Guid.NewGuid().ToString(),
                        Temp = float.Parse(tempEntry.Text),
                        FeelsLike = float.Parse(feelsLikeEntry.Text),
                        TempMin = float.Parse(tempMinEntry.Text),
                        TempMax = float.Parse(tempMaxEntry.Text),
                        Pressure = float.Parse(pressureEntry.Text),
                        Humidity = float.Parse(humidityEntry.Text),
                        SeaLevel = float.Parse(seaLevelEntry.Text),
                        GrndLevel = float.Parse(grndLevelEntry.Text)
                    }
                });
                await _localDbContext.SaveChangesAsync();

                Device.BeginInvokeOnMainThread(() => { _weatherDataListChanged?.Invoke(this, EventArgs.Empty); });
            });
        }

        private void OnDeleteWeatherDataClicked(object sender, EventArgs e)
        {
            Task.Run(async () =>
            {
                var weatherData = _localDbContext.WeatherDataSet.FirstOrDefault();
                if (weatherData != null)
                {
                    _localDbContext.Remove(weatherData);
                }

                await _localDbContext.SaveChangesAsync();   

                Device.BeginInvokeOnMainThread(() => { _weatherDataListChanged?.Invoke(this, EventArgs.Empty); });

            });
        }

        private void OnFetchWeatherDataClicked(object sender, EventArgs e)
        {
            Task.Run(async () =>
            {
                WeatherResponse response = await _weatherRepository.GetWeatherForCity("London", new CancellationToken());
                WeatherData data = response.Adapt<WeatherData>();

                await _localDbContext.AddAsync(data);
                await _localDbContext.SaveChangesAsync();

                Device.BeginInvokeOnMainThread(() => { _weatherDataListChanged?.Invoke(this, EventArgs.Empty); });
            });
        }
    }
}
