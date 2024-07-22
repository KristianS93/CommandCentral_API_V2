namespace API.GroceryList.Models;

public class CreateGroceryItemDto
{
    public string? ItemId { get; set; }
    public string Name { get; set; } 
    public string Amount { get; set; } 

    public CreateGroceryItemDto(string? itemId, string name, string amount)
    {
        ItemId = itemId;
        Name = name;
        Amount = amount;
    }
}