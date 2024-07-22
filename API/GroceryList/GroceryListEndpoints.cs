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

        groceryList.MapGet("/", async (ClaimsPrincipal principal, GroceryListService groceryListService) =>
        {
            var householdId = principal.FindFirst(Claims.Household)!.Value;
            var result = await groceryListService.GetItems(householdId);
            Console.WriteLine("number of items: " + result.Count);
            return Results.Ok(result);
        }).RequireAuthorization(Roles.Member);
        
        groceryList.MapHub<GroceryListHub>("/hub").RequireAuthorization(Roles.Member);
        
        // example:
        return app;
    }
}