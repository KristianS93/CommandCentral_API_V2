using System.ComponentModel.DataAnnotations;
using API.SharedAPI.Models;

namespace API.GroceryList.Models;

public class GroceryItemModel : BaseEntity
{
    [Required]
    public string ItemId { get; set; } = Guid.NewGuid().ToString();
    [Required]
    public string GroceryListId { get; set; } = string.Empty;
    public GroceryListModel? GroceryList { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    public string Amount { get; set; } = string.Empty;

    public string Picture { get; set; } = string.Empty;
}