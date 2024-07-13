using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace API.identity.Models;

public class CCAIdentity : IdentityUser
{
    [Required]
    public string Firstname { get; set; } = string.Empty;
    
    [Required]
    public string Lastname { get; set; } = string.Empty;

    public string HouseholdId { get; set; } = string.Empty;
}