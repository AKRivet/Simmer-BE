using SimmerBackend.Api.Features.Recipes;
using SimmerBackend.Api.Features.Recipes.Commands;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddSingleton<RecipeStore>();
builder.Services.AddSingleton<CreateRecipeHandler>();

builder.Services.AddControllers();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info = new()
        {
            Title = "Simmer Backend API",
            Version = "v1",
            Description = "REST API for the Simmer Backend service.",
            Contact = new() { Name = "Simmer Backend Team" },
        };
        return Task.CompletedTask;
    });
});

var app = builder.Build();

app.MapOpenApi("/openapi/v1.json");

app.UseCors("AllowFrontend");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

// Partial class required for WebApplicationFactory<Program> in integration tests
public partial class Program { }
