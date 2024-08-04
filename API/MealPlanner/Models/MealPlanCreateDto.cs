namespace API.MealPlanner.Models;

public record MealPlanCreateDto(int Year, int Week, List<MealInPlanDto>? MealIds);