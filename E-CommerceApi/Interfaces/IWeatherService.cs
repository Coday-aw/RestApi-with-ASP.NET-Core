using E_CommerceApi.DTOs.WeatherAPI;

namespace E_CommerceApi.Interfaces;

public interface IWeatherService
{
    Task<WeatherApiDto> GetCurrentWeatherAsync(string city);
}