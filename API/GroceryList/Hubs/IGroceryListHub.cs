using API.GroceryList.Models;
using FluentResults;

namespace API.GroceryList.Hubs;

public interface IGroceryListHub
{
    // Task SendAsync(string eventItem, string msg);
    Task JoinGroceryList(string connected);
    Task AddItem(CreateGroceryItemDto item);

    Task EditItem(GroceryItemDto item);

    Task DeleteItem(string itemId);

    Task GetItems(List<GroceryItemDto> items);

    Task Error(List<IError> error);
}