using System.Collections.Concurrent;
using SimmerBackend.Api.Models;

namespace SimmerBackend.Api.Features.Recipes;

/// <summary>Thread-safe in-memory store for recipes.</summary>
public sealed class RecipeStore
{
    private readonly ConcurrentDictionary<Guid, Recipe> _recipes = new();

    public Recipe Add(Recipe recipe)
    {
        _recipes[recipe.Id] = recipe;
        return recipe;
    }

    public IReadOnlyList<Recipe> GetAll() =>
        [.. _recipes.Values];

    public Recipe? GetById(Guid id) =>
        _recipes.TryGetValue(id, out var recipe) ? recipe : null;
}
