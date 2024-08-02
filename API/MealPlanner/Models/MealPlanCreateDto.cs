namespace API.MealPlanner.Models;

public record MealPlanCreateDto(string HouseholdId, DateTime Week, List<MealInPlanDto>? MealIds);