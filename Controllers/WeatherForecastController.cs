using Microsoft.AspNetCore.Mvc;
using ActiveMQ.Artemis.Client;
using active_mq_app.Model;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace active_mq_app.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpPost(Name = "CreateQueue")]
    public async Task<ActionResult> Post(Todo todo)
    {
        var connectionFactory = new ConnectionFactory();
        var endpoint = ActiveMQ.Artemis.Client.Endpoint.Create("localhost", 5672, "admin", "admin");
        var connection = await connectionFactory.CreateAsync(endpoint);

        var producer = await connection.CreateProducerAsync("a1", RoutingType.Anycast);

        await producer.SendAsync(new Message("foo"));

        return Ok();
    }

}
