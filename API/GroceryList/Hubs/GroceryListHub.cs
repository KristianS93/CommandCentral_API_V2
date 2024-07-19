using System.Security.Claims;
using System.Text.Json;
using API.GroceryList.Models;
using API.Identity;
using API.identity.Models;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace API.GroceryList.Hubs;

public class GroceryListHub : Hub<IGroceryListHub>
{
    private readonly UserManager<CCAIdentity> _userManager;
    private readonly GroceryListService _groceryListService;
    private readonly SharedConnections _connections;

    
    public GroceryListHub(UserManager<CCAIdentity> userManager, GroceryListService groceryListService, SharedConnections connections)
    {
        _userManager = userManager;
        _groceryListService = groceryListService;
        _connections = connections;
    }
    public override async Task OnConnectedAsync()
    {
        // working
        var householdId = Context.User!.FindFirst(Claims.Household)!.Value;
        var userId = Context.User!.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        
        _connections.Connections[Context.ConnectionId] =
            new UserConnection(userId, householdId);
        await Groups.AddToGroupAsync(Context.ConnectionId, householdId);
        var items = await _groceryListService.GetItems(householdId);
        Console.WriteLine("number of items: " + items.Count);
        // var jsonItems = JsonSerializer.Serialize(items);
        await GetItems("GetItems", items);
        // Console.WriteLine(jsonItems);
        Console.WriteLine($"Connected to group {householdId}, user: {userId}");
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        // working
        Console.WriteLine("Connections before " + _connections.Connections.Count);
        var connection = _connections.Connections[Context.ConnectionId];
        _connections.Connections.Remove(Context.ConnectionId, out connection);
        Console.WriteLine("Connections after" + _connections.Connections.Count);
        return base.OnDisconnectedAsync(exception);
    }

    public async Task GetItems(string eventItem, List<GroceryItemDto> items)
    {
        foreach (var item in items)
        {
            Console.WriteLine(item.Name);
        }
        if (_connections.Connections.TryGetValue(Context.ConnectionId, out UserConnection? connection))
        {
            await Clients.Group(connection.HousehouldId).GetItems(eventItem, items);
        }
    }
    public async Task AddItem(string eventItem, CreateGroceryItemDto item)
    {
        if (_connections.Connections.TryGetValue(Context.ConnectionId, out UserConnection? connection))
        {
            var result = await _groceryListService.AddItem(item, connection.HousehouldId);
            if (result.IsFailed)
            {
                await Error("Error", result.Errors);
            }
            else
            {
                await Clients.Group(connection.HousehouldId).AddItem(eventItem, item);
            }
        }
    }
    public async Task Error(string eventItem, List<IError> error)
    {
        if (_connections.Connections.TryGetValue(Context.ConnectionId, out UserConnection? connection))
        {
            await Clients.Group(connection.HousehouldId).Error(eventItem, error);
        }
    }
    
    public async Task EditItem(string eventItem, GroceryItemDto item){
        if (_connections.Connections.TryGetValue(Context.ConnectionId, out UserConnection? connection))
        {
            var result = await _groceryListService.EditItem(item, connection.HousehouldId);
            if (result.IsFailed)
            {
                await Error("Error", result.Errors);
            }
            else
            {
                await Clients.Group(connection.HousehouldId).EditItem(eventItem, item);
            }
        }
    }

    public async Task DeleteItem(string eventItem, string itemId){
        if (_connections.Connections.TryGetValue(Context.ConnectionId, out UserConnection? connection))
        {
            var result = await _groceryListService.DeleteItem(itemId, connection.HousehouldId);
            if (result.IsFailed)
            {
                await Error("Error", result.Errors);
            }
            else
            {
                await Clients.Group(connection.HousehouldId).DeleteItem(eventItem, itemId);
            }
        }
    }
}