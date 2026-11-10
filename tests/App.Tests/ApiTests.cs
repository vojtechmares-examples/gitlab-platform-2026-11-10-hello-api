using System.Net;
using System.Net.Http.Json;
using App.Controllers;
using Microsoft.AspNetCore.Mvc.Testing;

namespace App.Tests;

public class HelloControllerTests
{
    [Theory]
    [InlineData(null, "Hello, world!")]
    [InlineData("  ", "Hello, world!")]
    [InlineData(" Ada ", "Hello, Ada!")]
    public void Greets_by_name(string? name, string expected)
    {
        Assert.Equal(expected, new HelloController().Get(name).Message);
    }
}

public class ApiTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Hello_returns_a_greeting()
    {
        var greeting = await _client.GetFromJsonAsync<Greeting>("/api/hello?name=Grace");

        Assert.Equal("Hello, Grace!", greeting!.Message);
    }

    [Fact]
    public async Task Health_is_ok()
    {
        Assert.Equal(HttpStatusCode.OK, (await _client.GetAsync("/health")).StatusCode);
    }

    [Fact]
    public async Task Swagger_describes_the_api()
    {
        var json = await _client.GetStringAsync("/swagger/v1/swagger.json");

        Assert.Contains("/api/Hello", json);
    }
}
