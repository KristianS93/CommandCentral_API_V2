using System.ComponentModel.DataAnnotations;
using API.identity.Models;

namespace API.Household.Models;

public class HouseholdUsersModel
{
    [Required]
    public string HouseholdId { get; set; } = string.Empty;
    public HouseholdModel Household { get; set; } = new HouseholdModel();
    [Required]
    public string UserId { get; set; } = string.Empty;
    public CCAIdentity User { get; set; } = new CCAIdentity();

    public string Role { get; set; } = string.Empty;
}