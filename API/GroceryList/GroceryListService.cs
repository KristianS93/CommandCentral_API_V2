using API.GroceryList.Models;
using API.SharedAPI.Persistence;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API.GroceryList;

public class GroceryListService
{
    private readonly ApiDbContext _apiDbContext;

    public GroceryListService(ApiDbContext apiDbContext)
    {
        _apiDbContext = apiDbContext;
    }

    public async Task<List<GroceryItemDto>> GetItems(string householdId)
    {
        var items = await _apiDbContext.GroceryLists.Include(k => k.Items)
            .FirstOrDefaultAsync(k => k.HousehouldId == householdId);
        
        var returnItems = items!.Items!.Select(i => new GroceryItemDto(i.ItemId, i.Name, i.Amount)).ToList();
        return returnItems;
    }

    public async Task<Result<GroceryItemModel>> AddItem(CreateGroceryItemDto item, string househouldId)
    {
        if (item.Name.IsNullOrEmpty())
        {
            return Result.Fail(new Error("No name given").WithMetadata("NameError", item.Name)
                .CausedBy("Missing item name"));
        }

        var list = await _apiDbContext.GroceryLists.Include(k => k.Items).
            FirstOrDefaultAsync(k => k.HousehouldId == househouldId);

        var newItem = new GroceryItemModel
        {
            GroceryListId = list!.GroceryListId,
            Name = item.Name,
            Amount = item.Amount,
            Picture = string.Empty,
        };
        list!.Items!.Add(newItem);
        await _apiDbContext.SaveChangesAsync();
        return Result.Ok(newItem);
    }

    public async Task<Result> EditItem(GroceryItemDto item, string househouldId)
    {
        if (item.ItemId.IsNullOrEmpty() || item.Name.IsNullOrEmpty())
        {
            return Result.Fail(new Error("Missing id or name.").WithMetadata("MissingProperty", item.Name)
                .CausedBy("Missing item name or id."));
        }
        var list = await _apiDbContext.GroceryLists.Include(k => k.Items).
            FirstOrDefaultAsync(k => k.HousehouldId == househouldId);

        var itemToChange = list!.Items!.First(k => k.ItemId == item.ItemId);
        itemToChange.Name = item.Name;
        itemToChange.Amount = item.Amount;

        await _apiDbContext.SaveChangesAsync();
        return Result.Ok();
    }

    public async Task<Result> DeleteItem(string itemId, string househouldId)
    {
        if (itemId.IsNullOrEmpty())
        {
            return Result.Fail(new Error("Missing id").WithMetadata("MissingProperty", itemId)
                .CausedBy("Missing id."));
        }
        var list = await _apiDbContext.GroceryLists.Include(k => k.Items).
            FirstOrDefaultAsync(k => k.HousehouldId == househouldId);

        var deleteItem = list!.Items!.First(k => k.ItemId == itemId);
        list!.Items!.Remove(deleteItem);
        await _apiDbContext.SaveChangesAsync();
        return Result.Ok();
    }
}