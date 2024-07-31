using System.ComponentModel.DataAnnotations;

namespace API.MealPlanner.Models;

public class MealsInPlan
{
    public string MealsInPlanId { get; set; } = Guid.NewGuid().ToString();
    [Required] public string MealId { get; set; } = string.Empty;
    public MealModel? Meal { get; set; }
    [Required] public string MealPlanId { get; set; } = string.Empty;
    public MealPlanModel? MealPlan { get; set; }
}