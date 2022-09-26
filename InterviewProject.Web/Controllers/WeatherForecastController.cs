using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using InterviewProject.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InterviewProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        public HttpClient _client;
       

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _client = new HttpClient();
            _logger = logger;
        }
        private static readonly string[] Summaries = new[]
      {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now,
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
        public class DataReact
        {
            public string place { set; get; }
        }
        [HttpPost]
        public async Task<IEnumerable<WeatherForecast>> PostAsync(DataReact request)
        {

            var baseUrl = "https://api.openweathermap.org/data/2.5/forecast";
            _client.BaseAddress = new Uri(baseUrl);
            var url = "?q="+request.place+"&appid=" + "f21182032d4a76b94d2022ae0b58d853";

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var result = await _client.GetAsync(url);
            var resultList = await JsonSerializer.DeserializeAsync<Root>(await result.Content.ReadAsStreamAsync());
            return resultList.list.Select(index => new WeatherForecast
            {
                Date = DateTime.ParseExact(index.dt_txt, "yyyy-MM-dd HH:mm:ss",
                System.Globalization.CultureInfo.InvariantCulture),
                TemperatureC = Math.Round(KelvinCensius(index.main.temp,true),2),
                Summary = index.weather[0].description
            }).Take(5)
            .ToArray();
        }
        private static double KelvinCensius(double d, bool kelvin = false)
        {
            return kelvin ? (d - 273.15) : (d + 273.15);
        }
        public DateTime UnixTimeStampToDateTime(int unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }
    }
}
