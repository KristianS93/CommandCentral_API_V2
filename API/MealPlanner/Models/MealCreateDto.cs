namespace API.MealPlanner.Models;

public record MealCreateDto(string Name, string? Description, string HouseholdId);