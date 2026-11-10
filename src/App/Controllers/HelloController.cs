using Microsoft.AspNetCore.Mvc;

namespace App.Controllers;

/// <summary>The sample endpoint of the template: replace it with your own controllers.</summary>
[ApiController]
[Route("api/[controller]")]
public class HelloController : ControllerBase
{
    /// <summary>Greets the caller.</summary>
    /// <param name="name">Who to greet (default: world).</param>
    [HttpGet]
    public Greeting Get([FromQuery] string? name) => new($"Hello, {(string.IsNullOrWhiteSpace(name) ? "world" : name.Trim())}!");
}

public record Greeting(string Message);
