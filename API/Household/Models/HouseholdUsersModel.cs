using API.identity.Models;

namespace API.Household.Models;

public class HouseholdUsersModel
{
    public string HouseholdId { get; set; } = string.Empty;
    public HouseholdModel Household { get; set; } = new HouseholdModel();
    public string UserId { get; set; } = string.Empty;
    public CCAIdentity User { get; set; } = new CCAIdentity();
}