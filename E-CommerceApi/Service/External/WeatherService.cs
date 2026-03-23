using System.Text.Json;
using E_CommerceApi.DTOs.WeatherAPI;
using E_CommerceApi.Interfaces;
using E_CommerceApi.Models;

namespace E_CommerceApi.Service.External;

public class WeatherService :  IWeatherService
{
    private readonly HttpClient _client;
    private readonly string _apiKey;
    
    public WeatherService(HttpClient client, IConfiguration config)
    {
        _client = client;
        _apiKey = config["WeatherApi:ApiKey"];
    }

    public async Task<WeatherApiDto> GetCurrentWeatherAsync(string city)
    {
        var url = $"weather?q={city}&appid={_apiKey}&units=metric";

        var response = await _client.GetAsync(url);
        var json = await response.Content.ReadAsStringAsync();
        
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"OpenWeather error: {json}");
        }
        
        var data = JsonSerializer.Deserialize<OpenWeatherResponse>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (data == null)
        {
            throw new Exception("Failed to parse OpenWeather response");
        }
        
        return new WeatherApiDto
        {
            City = data.Name,
            Temperature = data.Main?.Temp ?? 0,
            Description = data.Weather?.FirstOrDefault()?.Description ?? "No description",
            WindSpeed = data.Wind?.Speed ?? 0
        };
    }

    
}