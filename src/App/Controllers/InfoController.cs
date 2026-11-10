using Microsoft.AspNetCore.Mvc;

namespace App.Controllers;

/// <summary>Which build runs where: the first thing to look at after a deployment.</summary>
[ApiController]
[Route("api/[controller]")]
public class InfoController(IWebHostEnvironment environment, IConfiguration configuration) : ControllerBase
{
    /// <summary>Build (the pipeline id), environment and runtime.</summary>
    [HttpGet]
    public object Get() => new
    {
        application = environment.ApplicationName,
        build = configuration["APP_BUILD"] ?? "local",
        environment = environment.EnvironmentName,
        host = Environment.MachineName,
        runtime = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription,
        time = DateTimeOffset.UtcNow,
    };
}
