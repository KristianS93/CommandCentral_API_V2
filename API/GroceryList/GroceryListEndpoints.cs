using System.Security.Claims;
using API.GroceryList.Hubs;
using API.identity;
using API.Identity;
using Microsoft.AspNetCore.SignalR;

namespace API.GroceryList;

public static class GroceryListEndpoints
{
    public static WebApplication AddGroceryListEndpoints(this WebApplication app)
    {
        var groceryList = app.MapGroup("/grocerylist").WithOpenApi().WithTags("GroceryList");
        
        groceryList.MapPost("/", async (ClaimsPrincipal principal, IHubContext<GroceryListHub, IGroceryListHub> context) =>
        {
            // add item
            // require a connection to group is active
            await Task.Delay(1);
        });
        
        groceryList.MapPut("/", async (ClaimsPrincipal principal, IHubContext<GroceryListHub, IGroceryListHub> context) =>
        {
            // add item
            // require a connection to group is active
            await Task.Delay(1);
        });
        
        groceryList.MapDelete("/", async (ClaimsPrincipal principal, IHubContext<GroceryListHub, IGroceryListHub> context) =>
        {
            // add item
            // require a connection to group is active
            await Task.Delay(1);
        });
        
        // groceryList.MapGet("/", async (ClaimsPrincipal principal, IHubContext<GroceryListHub, IGroceryListHub> context) =>
        // {
        //     // add item
        //     // require a connection to group is active
        //     await Task.Delay(1);
        // });
        
        groceryList.MapHub<GroceryListHub>("/hub").RequireAuthorization(Roles.Member);
        return app;
    }
}