namespace API.MealPlanner.Models;

public record MealPlanEditDto(string MealPlanid, int Year, int Week, List<MealInPlanDto>? MealIds);