using Microsoft.AspNetCore.Mvc;
using NoRslinx.Application.WeatherForecasts.Queries.GetWeatherForecasts;

namespace WebApi.Controllers;
public class WeatherForecastController : ApiControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<WeatherForecast>> Get()
    {
        return await Mediator.Send(new GetWeatherForecastsQuery());
    }
}
