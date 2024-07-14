using API.Household.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Household;

public static class HouseholdEndpoints
{
    public static WebApplication AddHouseholdEndpoints(this WebApplication app)
    {
        var household = app.MapGroup("/household").WithOpenApi();

        household.MapPost("/", async ([FromBody]CreateHouseholdDTO household, HouseholdService householdService) =>
        {
            var result = await householdService.CreateHousehold(household);
            if (result.IsFailed)
            {
                return Results.BadRequest(result.Errors);
            }

            return Results.Created();
        });

        return app;
    }
}