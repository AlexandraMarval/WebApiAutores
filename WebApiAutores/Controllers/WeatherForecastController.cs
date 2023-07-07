using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace WebApiAutores.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class WeatherForecastController : ControllerBase
	{
		private static readonly string[] Summaries = new[]
		{
		"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
		};

		//private static readonly string[] Months = new[]
		//{
		//"Enero", "Febrero", "Marzo", "Abril", "MAyo", "Junio", "Julio", "Agostp", "Septiembre", "Octumbre",  "Noviembre",  "Diciembre"
		//};
		private readonly ILogger<WeatherForecastController> _logger;

		public WeatherForecastController(ILogger<WeatherForecastController> logger)
		{
			_logger = logger;
		}

		[HttpGet(Name = "GetWeatherForecast")]
		public IEnumerable<WeatherForecast> Get()
		{
			string[] monthNames = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;


			return Enumerable.Range(1, 5).Select(index => new WeatherForecast
			{
				Date = DateOnly.FromDateTime(DateTime.Now.AddMonths(index)),
				TemperatureC = Random.Shared.Next(-20, 55),
				Summary = Summaries[Random.Shared.Next(Summaries.Length)],
				Months = monthNames[DateTime.Now.AddMonths(index).Month - 1]
				//Months = Months[DateTime.Now.AddMonths(index).Month - 1]
			})
			.ToArray();

			//var range = Enumerable.Range(1, 5);

			//var response = new List<WeatherForecast>();

			//foreach (var index in range)
			//{
			//	var now = DateTime.Now;
			//	var date = now.AddMonths(index);

			//	var weather = new WeatherForecast();

			//	weather.Date = DateOnly.FromDateTime(date);
			//	weather.TemperatureC = Random.Shared.Next(-20, 55);
			//	weather.Summary = Summaries[Random.Shared.Next(Summaries.Length)];
			//	weather.Months = Months[date.Month - 1];

			//	response.Add(weather);				
			//}

			//return response;	
		}
	}
}