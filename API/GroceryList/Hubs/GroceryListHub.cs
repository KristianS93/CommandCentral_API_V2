using System.Security.Claims;
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
        // Context.User.FindFirst(Cla)
        // _connections.Connections[Context.ConnectionId] = new UserConnection()
        await Clients.All.RecieveMSG($"{Context.ConnectionId} {Context.User!.FindFirst(ClaimTypes.NameIdentifier)!.Value} connected!");
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var connection = _connections.Connections[Context.ConnectionId];
        _connections.Connections.Remove(Context.ConnectionId, out connection);
        return base.OnDisconnectedAsync(exception);
    }

    public async Task ConnectGroceryList(string eventItem, string msg)
    {
        Console.WriteLine("try to connect to hub");
        // var householdId = _principal.FindFirst(Claims.Household)!.Value;
        // if (householdId == Claims.HouseholdDefault)
        // {
        //     throw new ArgumentNullException("error");
        // }
        // await Groups.AddToGroupAsync(Context.ConnectionId, _principal.FindFirst(Claims.Household)!.Value);
        _connections.Connections[Context.ConnectionId] =
            new UserConnection("myName", "name");
        await Groups.AddToGroupAsync(Context.ConnectionId, "name");
        // var groceryList = new GroceryListModel
        // {
        //     HousehouldId = householdId,
        //     Items = new List<GroceryItemModel>()
        // };
        // groceryList.Items.Add(new GroceryItemModel {Amount = "1", Name = "xd", GroceryListId = groceryList.GroceryListId});
        // groceryList.Items.Add(new GroceryItemModel {Amount = "2", Name = "xd3", GroceryListId = groceryList.GroceryListId});
        await Clients.Group("name").ConnectGroceryList("ConnectGroceryList", $"Connected to name room");
    }

    public async Task AddItem(string eventItem, string msg)
    {
        if (_connections.Connections.TryGetValue(Context.ConnectionId, out UserConnection? connection))
        {
            await Clients.Group(connection.HousehouldId).AddItem("AddItem", $"{connection.UserId} msg");
        }
        
    }

    // public async Task DeleteItem(GroceryItemModel item)
    // {
    //     await Task.Delay(1);
    // }
    //
    // public async Task Edititem(GroceryItemModel item)
    // {
    //     await Task.Delay(1);
    // }
    
    
}