namespace E_CommerceApi.DTOs.WeatherAPI;

public record WeatherApiDto
{
    public string City { get; set; }
    public double Temperature { get; set; }
    public string Description { get; set; }
    public double WindSpeed { get; set; }
    
}