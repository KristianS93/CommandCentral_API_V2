using API.GroceryList.Models;
using FluentResults;

namespace API.GroceryList.Hubs;

public interface IGroceryListHub
{
    Task AddItem(string eventItem, CreateGroceryItemDto item);

    Task EditItem(string eventItem, GroceryItemDto item);

    Task DeleteItem(string eventItem, string itemId);

    Task GetItems(string eventItem, List<GroceryItemDto> items);

    Task Error(string eventItem, List<IError> error);
}