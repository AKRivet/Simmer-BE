using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace SimmerBackend.Api.Tests;

public sealed class RecipesEndpointTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    [Fact]
    public async Task PostRecipe_WithValidData_Returns201()
    {
        var request = new
        {
            name = "Pasta Primavera",
            cuisine = "Italian",
            prepTimeMinutes = 20,
            ingredients = new[] { "pasta", "olive oil", "garlic" },
            steps = new[] { "Boil pasta", "Sauté garlic in oil", "Combine and serve" },
        };

        HttpResponseMessage response = await _client.PostAsJsonAsync("/recipes", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task PostRecipe_WithValidData_ReturnsRecipeBody()
    {
        var request = new
        {
            name = "Pasta Primavera",
            cuisine = "Italian",
            prepTimeMinutes = 20,
            ingredients = new[] { "pasta", "olive oil" },
            steps = new[] { "Boil pasta", "Combine" },
        };

        HttpResponseMessage response = await _client.PostAsJsonAsync("/recipes", request);
        string body = await response.Content.ReadAsStringAsync();
        using JsonDocument doc = JsonDocument.Parse(body);

        Assert.Equal("Pasta Primavera", doc.RootElement.GetProperty("name").GetString());
        Assert.Equal("Italian", doc.RootElement.GetProperty("cuisine").GetString());
        Assert.Equal(20, doc.RootElement.GetProperty("prepTimeMinutes").GetInt32());
        Assert.True(doc.RootElement.GetProperty("id").GetString()?.Length > 0);
    }

    [Fact]
    public async Task PostRecipe_MissingName_Returns400()
    {
        var request = new
        {
            name = "",
            cuisine = "Italian",
            prepTimeMinutes = 20,
            ingredients = new[] { "pasta" },
            steps = new[] { "Boil pasta" },
        };

        HttpResponseMessage response = await _client.PostAsJsonAsync("/recipes", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task PostRecipe_MissingCuisine_Returns400()
    {
        var request = new
        {
            name = "Pasta",
            cuisine = "",
            prepTimeMinutes = 20,
            ingredients = new[] { "pasta" },
            steps = new[] { "Boil pasta" },
        };

        HttpResponseMessage response = await _client.PostAsJsonAsync("/recipes", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task PostRecipe_ZeroPrepTime_Returns400()
    {
        var request = new
        {
            name = "Pasta",
            cuisine = "Italian",
            prepTimeMinutes = 0,
            ingredients = new[] { "pasta" },
            steps = new[] { "Boil pasta" },
        };

        HttpResponseMessage response = await _client.PostAsJsonAsync("/recipes", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task PostRecipe_EmptyIngredients_Returns400()
    {
        var request = new
        {
            name = "Pasta",
            cuisine = "Italian",
            prepTimeMinutes = 20,
            ingredients = Array.Empty<string>(),
            steps = new[] { "Boil pasta" },
        };

        HttpResponseMessage response = await _client.PostAsJsonAsync("/recipes", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task PostRecipe_EmptySteps_Returns400()
    {
        var request = new
        {
            name = "Pasta",
            cuisine = "Italian",
            prepTimeMinutes = 20,
            ingredients = new[] { "pasta" },
            steps = Array.Empty<string>(),
        };

        HttpResponseMessage response = await _client.PostAsJsonAsync("/recipes", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task PostRecipe_MissingFields_ReturnsValidationErrors()
    {
        var request = new
        {
            name = "",
            cuisine = "",
            prepTimeMinutes = 20,
            ingredients = new[] { "pasta" },
            steps = new[] { "Boil" },
        };

        HttpResponseMessage response = await _client.PostAsJsonAsync("/recipes", request);
        string body = await response.Content.ReadAsStringAsync();
        using JsonDocument doc = JsonDocument.Parse(body);

        Assert.True(doc.RootElement.TryGetProperty("errors", out _));
    }
}
