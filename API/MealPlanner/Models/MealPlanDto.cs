namespace API.MealPlanner.Models;

public record MealPlanDto(string MealPlanId, DateTime Week, List<MealInfoDto> Meals);