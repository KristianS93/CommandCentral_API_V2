using System.ComponentModel.DataAnnotations;
using API.identity.Models;
using API.SharedAPI.Models;

namespace API.Household.Models;

public class HouseholdModel : BaseEntity
{
    [Required]
    public string HouseholdId { get; set; } = string.Empty;
    [Required]
    public string Name { get; set; } = string.Empty;
}