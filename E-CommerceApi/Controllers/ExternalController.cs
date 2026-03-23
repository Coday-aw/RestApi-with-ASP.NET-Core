using E_CommerceApi.Interfaces;
using E_CommerceApi.Service.External;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExternalController : ControllerBase
{
    private readonly JokeApiClient _apiClient;
    private readonly IWeatherService  _weatherService;

    public ExternalController(JokeApiClient apiClient,  IWeatherService weatherService)
    {
        _apiClient = apiClient;
        _weatherService = weatherService;
    }

    [HttpGet("jokes")]
    public async Task<IActionResult> GetJoke()
    {
        var joke = await _apiClient.GetRandomJokeAsync();
        return Ok(joke);
    }

    [HttpGet("weather")]
    public async Task<IActionResult> GetWeather([FromQuery] string city)
    {
        if(string.IsNullOrWhiteSpace(city))
            return BadRequest("City is required");
        var result = await _weatherService.GetCurrentWeatherAsync(city);
        
        return Ok(result);
    }
}