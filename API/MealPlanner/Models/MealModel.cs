using System.ComponentModel.DataAnnotations;
using API.SharedAPI.Models;

namespace API.MealPlanner.Models;

public class MealModel : BaseEntity
{
    public string MealId { get; set; } = Guid.NewGuid().ToString();
    [Required] [MaxLength(50)] public string Name { get; set; } = string.Empty;
    [Required] public string Description { get; set; } = string.Empty;
    public string? Image { get; set; } = string.Empty;

    public ICollection<IngredientModel>? Ingredients { get; set; }
    
}