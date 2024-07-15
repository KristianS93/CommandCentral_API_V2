using System.ComponentModel.DataAnnotations;
using API.identity.Models;

namespace API.Household.Models;

public class HouseholdUsersModel
{
    [Required]
    public string HouseholdId { get; set; } = string.Empty;
    
    [Required]
    public string UserId { get; set; } = string.Empty;
    
    [Required]
    public string Role { get; set; } = string.Empty;
}