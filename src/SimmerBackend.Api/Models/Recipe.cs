namespace SimmerBackend.Api.Models;

/// <summary>Represents a stored recipe.</summary>
public sealed record Recipe(
    Guid Id,
    string Name,
    string Cuisine,
    int PrepTimeMinutes,
    IReadOnlyList<string> Ingredients,
    IReadOnlyList<string> Steps
);
