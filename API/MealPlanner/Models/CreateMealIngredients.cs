namespace API.MealPlanner.Models;

public record CreateMealIngredients(string Name, string? Description, List<CreateMealIngredientItemDto>? Ingredients);