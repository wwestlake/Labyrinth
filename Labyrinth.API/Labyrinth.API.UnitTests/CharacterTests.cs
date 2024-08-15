using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Labyrinth;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Labyrinth.API.UnitTests;

public class CharacterTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public CharacterTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetEntities_ReturnsSuccess()
    {
        // Act
        var response = await _client.GetAsync("/api/character");

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Contains("Test Entity 1", responseString);
    }
}