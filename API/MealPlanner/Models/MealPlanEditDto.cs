namespace API.MealPlanner.Models;

public record MealPlanEditDto(string MealPlanid, DateTime Week, List<MealInPlanDto>? MealIds);