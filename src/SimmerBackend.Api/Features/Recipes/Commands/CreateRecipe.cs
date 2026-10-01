using SimmerBackend.Api.Models;

namespace SimmerBackend.Api.Features.Recipes.Commands;

public sealed record CreateRecipeCommand(
    string Name,
    string Cuisine,
    int PrepTimeMinutes,
    IReadOnlyList<string> Ingredients,
    IReadOnlyList<string> Steps
);

public sealed class CreateRecipeHandler(RecipeStore store)
{
    public Recipe Handle(CreateRecipeCommand command) =>
        store.Add(new Recipe(
            Guid.NewGuid(),
            command.Name,
            command.Cuisine,
            command.PrepTimeMinutes,
            command.Ingredients,
            command.Steps
        ));
}
