using System.ComponentModel.DataAnnotations;

namespace API.MealPlanner.Models;

public record MealEditDto(string MealId, [MaxLength(50)] string Name, string Description);