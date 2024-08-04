namespace API.MealPlanner.Models;

public record MealPlanDto(string MealPlanId, int Year, int Week, List<MealInfoDto> Meals);