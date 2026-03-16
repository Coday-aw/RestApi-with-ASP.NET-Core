using E_CommerceApi.Service.External;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExternalController : ControllerBase
{
    private readonly JokeApiClient _apiClient;

    public ExternalController(JokeApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [HttpGet("jokes/random")]
    public async Task<IActionResult> GetJoke()
    {
        var joke = await _apiClient.GetRandomJokeAsync();
        return Ok(joke);
    }
}