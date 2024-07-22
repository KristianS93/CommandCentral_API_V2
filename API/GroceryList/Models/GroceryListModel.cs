using System.ComponentModel.DataAnnotations;
using API.Household.Models;
using API.SharedAPI.Models;

namespace API.GroceryList.Models;

public class GroceryListModel : BaseEntity
{
    [Required]
    public string GroceryListId { get; set; } = Guid.NewGuid().ToString();
    [Required]
    public string HousehouldId { get; set; } = string.Empty;

    public HouseholdModel? Household { get; set; }

    public List<GroceryItemModel>? Items { get; set; }
    
}