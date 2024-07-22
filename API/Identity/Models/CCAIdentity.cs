using System.ComponentModel.DataAnnotations;
using API.Household.Models;
using Microsoft.AspNetCore.Identity;

namespace API.identity.Models;

public class CCAIdentity : IdentityUser
{
    [Required]
    [MinLength(1)]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    [MinLength(1)]
    public string LastName { get; set; } = string.Empty;
}