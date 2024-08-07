namespace API.MealPlanner.Models;

public record MealDto(string MealId, string Name, string Description, List<MealIngredientDto> Ingredients);