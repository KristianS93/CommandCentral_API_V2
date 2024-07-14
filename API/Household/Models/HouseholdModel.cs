using System.ComponentModel.DataAnnotations;
using API.identity.Models;
using API.SharedAPI.Models;

namespace API.Household.Models;

public class HouseholdModel : BaseEntity
{
    public string HouseholdId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    public ICollection<HouseholdUsersModel> HouseholdUsers { get; set; } = new List<HouseholdUsersModel>();
}