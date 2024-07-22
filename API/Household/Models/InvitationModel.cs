using System.ComponentModel.DataAnnotations;
using API.SharedAPI.Models;

namespace API.Household.Models;

public class InvitationModel : BaseEntity
{
    public string InvitationId { get; set; } = Guid.NewGuid().ToString();
    
    [Required]
    public string InviteeUserId { get; set; } = string.Empty;
    
    [Required]
    public string InviterUserId { get; set; } = string.Empty;
    
    [Required]
    public string HouseholdId { get; set; } = string.Empty;
    public HouseholdModel? Household { get; set; }
}