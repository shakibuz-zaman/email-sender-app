using EmailService.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmailSenderApp.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly IEmailSender _emailSender;
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IEmailSender emailSender)
    {
        _logger = logger;
        _emailSender = emailSender;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        var message = new Message(new string[] { "test@gmail.com" }, "Test email", "This is the content from our email.", null);
        _emailSender.SendEmailAsync(message);
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
    [HttpPost(Name = "SendMail")]
    public async Task<IEnumerable<WeatherForecast>> Post()
    {
        var rng = new Random();

        var files = Request.Form.Files.Any() ? Request.Form.Files : new FormFileCollection();

        var message = new Message(new string[] { "test@gmail.com" }, "Test mail with Attachments", "This is the content from our mail with attachments.", files);
        await _emailSender.SendEmailAsync(message);

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}

