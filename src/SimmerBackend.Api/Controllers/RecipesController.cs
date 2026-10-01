using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SimmerBackend.Api.Features.Recipes.Commands;
using SimmerBackend.Api.Models;

namespace SimmerBackend.Api.Controllers;

/// <summary>Request body for creating a recipe.</summary>
public sealed class CreateRecipeRequest
{
    [Required(ErrorMessage = "Name is required.")]
    [MinLength(1, ErrorMessage = "Name cannot be empty.")]
    public string Name { get; init; } = string.Empty;

    [Required(ErrorMessage = "Cuisine is required.")]
    [MinLength(1, ErrorMessage = "Cuisine cannot be empty.")]
    public string Cuisine { get; init; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Prep time must be at least 1 minute.")]
    public int PrepTimeMinutes { get; init; }

    [Required(ErrorMessage = "Ingredients are required.")]
    public IReadOnlyList<string> Ingredients { get; init; } = [];

    [Required(ErrorMessage = "Steps are required.")]
    public IReadOnlyList<string> Steps { get; init; } = [];
}

/// <summary>Manages recipe resources.</summary>
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public sealed class RecipesController(CreateRecipeHandler handler) : ControllerBase
{
    /// <summary>Creates a new recipe.</summary>
    /// <response code="201">Recipe created successfully.</response>
    /// <response code="400">Validation error — one or more required fields are missing or invalid.</response>
    [HttpPost]
    [ProducesResponseType(typeof(Recipe), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] CreateRecipeRequest request)
    {
        if (request.Ingredients.Count == 0)
            ModelState.AddModelError(nameof(CreateRecipeRequest.Ingredients), "At least one ingredient is required.");

        if (request.Steps.Count == 0)
            ModelState.AddModelError(nameof(CreateRecipeRequest.Steps), "At least one step is required.");

        if (!ModelState.IsValid)
            return ValidationProblem();

        Recipe recipe = handler.Handle(new CreateRecipeCommand(
            request.Name,
            request.Cuisine,
            request.PrepTimeMinutes,
            request.Ingredients,
            request.Steps
        ));

        return Created($"/recipes/{recipe.Id}", recipe);
    }
}
