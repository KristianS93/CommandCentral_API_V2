using System.ComponentModel.DataAnnotations;
using API.Household.Models;
using API.SharedAPI.Models;

namespace API.MealPlanner.Models;

public class MealPlanModel : BaseEntity
{
    public string MealPlanId { get; set; } = Guid.NewGuid().ToString();
    [Required] public string HouseholdId { get; set; } = String.Empty;
    public HouseholdModel? Household { get; set; }
    [Required] public DateTime WeekNo { get; set; }

    public ICollection<MealsInPlan>? Meals { get; set; }
}