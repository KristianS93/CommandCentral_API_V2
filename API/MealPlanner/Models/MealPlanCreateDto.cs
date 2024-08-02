namespace API.MealPlanner.Models;

public record MealPlanCreateDto(DateTime Week, List<MealInPlanDto>? MealIds);