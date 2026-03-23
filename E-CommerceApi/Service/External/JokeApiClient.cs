using E_CommerceApi.Interfaces;

namespace E_CommerceApi.Service.External;

public class JokeApiClient 
{
    private readonly HttpClient _httpClient;
    public JokeApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetRandomJokeAsync()
    {
        var response = await _httpClient.GetAsync("jokes/random");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}
