using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xml = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
    if (File.Exists(xml)) options.IncludeXmlComments(xml);
});
builder.Services.AddHealthChecks();

var app = builder.Build();

// Swagger UI is on in every environment: /swagger
app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();

// Lets the integration tests reach the entry point (WebApplicationFactory<Program>)
public partial class Program;
