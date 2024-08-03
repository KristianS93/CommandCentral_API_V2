using System.ComponentModel.DataAnnotations;
using API.SharedAPI.Models;

namespace API.MealPlanner.Models;

public class IngredientModel : BaseEntity
{
    public string IngredientId { get; set; } = Guid.NewGuid().ToString();
    [Required] public string MealId { get; set; } = string.Empty;
    public MealModel? Meal { get; set; }
    [Required][MaxLength(50)] public string Name { get; set; } = string.Empty;
    [Required][MaxLength(50)] public string Amount { get; set; } = string.Empty;
}