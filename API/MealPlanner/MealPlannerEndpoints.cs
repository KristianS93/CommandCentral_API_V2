using System.Security.Claims;
using API.identity;
using API.Identity;
using API.MealPlanner.Models;
using API.MealPlanner.Services;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API.MealPlanner;

public static class MealPlannerEndpoints
{
    public static WebApplication AddMealPlannerEndpoints(this WebApplication app)
    {

        #region Ingredients
        
        var ingredients = app.MapGroup("/ingredients").WithOpenApi().WithTags("Ingredients");
        ingredients.MapGet("/{id}", async (string id, [FromBody]MealPlanAddMeal mealId, IngredientService service, ClaimsPrincipal principal) =>
        {
            var householdId = principal.FindFirst(Claims.Household)!.Value;
            var result = await service.GetIngredient(id, mealId.MealId, householdId);
            if (result.IsFailed)
            {
                return Results.BadRequest(result.Errors);
            }
            return Results.Ok(result.Value);
        }).RequireAuthorization(Roles.Member);
        
        ingredients.MapDelete("/{id}", async (string id, [FromBody]MealPlanAddMeal mealId, ClaimsPrincipal principal, IngredientService service) =>
        {
            var householdId = principal.FindFirst(Claims.Household)!.Value;
            var result = await service.DeleteIngredient(id, mealId.MealId, householdId);
            if (result.IsFailed)
            {
                return Results.BadRequest(result.Errors);
            }
            return Results.Ok();
        }).RequireAuthorization(Roles.Member);
        
        ingredients.MapPut("/", async ([FromBody]IngredientDto data, IngredientService service, ClaimsPrincipal principal) =>
        {
            var householdId = principal.FindFirst(Claims.Household)!.Value;
            var result = await service.EditIngredient(data, householdId);
            if (result.IsFailed)
            {
                return Results.BadRequest(result.Errors);
            }
            return Results.Ok();
        }).RequireAuthorization(Roles.Member);
        
        ingredients.MapPost("/", async ([FromBody]IngredientCreateDto data, IngredientService service, ClaimsPrincipal principal) =>
        {
            var householdId = principal.FindFirst(Claims.Household)!.Value;
            var result = await service.CreateIngredient(data, householdId);
            if (result.IsFailed)
            {
                return Results.BadRequest(result.Errors);
            }
            return Results.Created();
        }).RequireAuthorization(Roles.Member);
        

        #endregion

        #region Meals

        var meal = app.MapGroup("/meals").WithOpenApi().WithTags("Meals");

        meal.MapGet("/", async (ClaimsPrincipal principal, MealService service) =>
        {
            var householdId = principal.FindFirst(Claims.Household)!.Value;
            var result = await service.GetMeals(householdId);
            if (result.IsFailed)
            {
                return Results.BadRequest(result.Errors);
            }
            return Results.Ok(result.Value);
            
        }).RequireAuthorization(Roles.Member);
        
        meal.MapPut("/{id}", async ([FromBody] MealEditDto mealData, MealService service, ClaimsPrincipal principal) =>
        {
            var householdId = principal.FindFirst(Claims.Household)!.Value;
            var result = await service.EditMeal(mealData, householdId);
            if (result.IsFailed)
            {
                return Results.BadRequest(result.Errors);
            }
            return Results.Ok();
        }).RequireAuthorization(Roles.Member);
        
        meal.MapGet("/{id}", async (string id, MealService service, ClaimsPrincipal principal) =>
        {
            var householdId = principal.FindFirst(Claims.Household)!.Value;
            var result = await service.GetMealById(id, householdId);
            if (result.IsFailed)
            {
                return Results.BadRequest(result.Errors);
            }

            return Results.Ok(result.Value);
        }).RequireAuthorization(Roles.Member);
        
        meal.MapDelete("/{id}", async (string id, MealService service, ClaimsPrincipal principal) =>
        {
            var householdId = principal.FindFirst(Claims.Household)!.Value;
            var result = await service.DeleteMeal(id, householdId);
            if (result.IsFailed)
            {
                return Results.BadRequest(result.Errors);
            }

            return Results.Ok();
        }).RequireAuthorization(Roles.Member);
        
        meal.MapPost("/", async ([FromBody]MealCreateDto mealData, MealService service, ClaimsPrincipal principal) =>
        {
            var householdId = principal.FindFirst(Claims.Household)!.Value;
            var result = await service.CreateMeal(mealData, householdId);
            if (result.IsFailed)
            {
                return Results.BadRequest(result.Errors);
            }

            return Results.Created();
        }).RequireAuthorization(Roles.Member);

        #endregion

        #region MealPlan
        
        var mealplanner = app.MapGroup("/mealplanner").WithOpenApi().WithTags("Mealplanner");
        // get mealplans from household
        mealplanner.MapGet("/", async (ClaimsPrincipal principal, MealPlanService service) =>
        {
            var householdId = principal.FindFirst(Claims.Household)!.Value;
            var result = await service.GetMealPlans(householdId);
            if (result.IsFailed)
            {
                return Results.BadRequest(result.Errors);
            }
            return Results.Ok(result.Value);
        }).RequireAuthorization(Roles.Member);
        
        // add meal to mealplan
        mealplanner.MapPost("/{id}", async (string id, [FromBody]MealPlanAddMeal mealId, ClaimsPrincipal principal, MealPlanService service) =>
        {
            var householdId = principal.FindFirst(Claims.Household)!.Value;
            var result = await service.AddMealToPlan(id, householdId, mealId);
            if (result.IsFailed)
            {
                return Results.BadRequest(result.Errors);
            }
            return Results.Created();
        }).RequireAuthorization(Roles.Member);
        
        // get mealplan
        mealplanner.MapGet("/{id}", async (string mealPlanId, MealPlanService service, ClaimsPrincipal principal) =>
        {
            var householdId = principal.FindFirst(Claims.Household)!.Value;
            var result = await service.GetMealplanById(mealPlanId, householdId);
            if (result.IsFailed)
            {
                return Results.BadRequest(result.Errors);
            }
            return Results.Ok(result.Value);
        }).RequireAuthorization(Roles.Member);
        
        // create mealplan

        mealplanner.MapPost("/", async ([FromBody] MealPlanCreateDto data, MealPlanService service, ClaimsPrincipal principal) =>
        {
            var householdId = principal.FindFirst(Claims.Household)!.Value;
            if (householdId.IsNullOrEmpty())
            {
                return Results.BadRequest(new Error("missing id"));
            }
            var result = await service.CreateMealPlan(data, householdId);
            if (result.IsFailed)
            {
                return Results.BadRequest(result.Errors);
            }
            return Results.Created();
        }).RequireAuthorization(Roles.Member);
        
        // delete mealplan
        mealplanner.MapDelete("/{id}", async (string id, MealPlanService service, ClaimsPrincipal principal) =>
        {
            var householdId = principal.FindFirst(Claims.Household)!.Value;
            var result = await service.DeleteMealPlan(id, householdId);
            if (result.IsFailed)
            {
                return Results.BadRequest(result.Errors);
            }
            return Results.Ok();
        }).RequireAuthorization(Roles.Member);
        
        // edit mealplan

        mealplanner.MapPut("/{id}", async ([FromBody]MealPlanEditDto data, MealPlanService service, ClaimsPrincipal principal) =>
        {
            var householdId = principal.FindFirst(Claims.Household)!.Value;
            var result = await service.EditMealPlan(data, householdId);
            if (result.IsFailed)
            {
                return Results.BadRequest(result.Errors);
            }
            return Results.Ok();
        }).RequireAuthorization(Roles.Member);
        
        // clear mealplan
        
        mealplanner.MapDelete("/clear/{id}", async (string id, MealPlanService service, ClaimsPrincipal principal) =>
        {
            var householdId = principal.FindFirst(Claims.Household)!.Value;
            var result = await service.ClearMealPlan(id, householdId);
            if (result.IsFailed)
            {
                return Results.BadRequest(result.Errors);
            }
            return Results.Ok();
        }).RequireAuthorization(Roles.Member);
        
        // auto add x amount of meals from own meal library
        mealplanner.MapPost("/generate", async ([FromBody]AutoMealPlanDto data, MealPlanService service, ClaimsPrincipal principal) =>
        {
            var householdId = principal.FindFirst(Claims.Household)!.Value;
            var result = await service.GenerateMealPlan(data, householdId);
            if (result.IsFailed)
            {
                return Results.BadRequest(result.Errors);
            }
            return Results.Created();
        }).RequireAuthorization(Roles.Member);
        
        // transfer to grocery list.
        mealplanner.MapPut("/groceries/{id}", async (string mealplanId, MealPlanService service, ClaimsPrincipal principal) =>
        {
            var householdId = principal.FindFirst(Claims.Household)!.Value;
            
            var result = await service.TransferMealPlan(mealplanId, householdId);
            if (result.IsFailed)
            {
                return Results.BadRequest(result.Errors);
            }
            return Results.Ok();
        }).RequireAuthorization(Roles.Member);
        
        #endregion
        
        return app;
    }
}