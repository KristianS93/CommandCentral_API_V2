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
        ingredients.MapGet("/{id}", async (string id, IngredientService service) =>
        {
            var result = await service.GetIngredient(id);
            if (result.IsFailed)
            {
                return Results.BadRequest(result.Errors);
            }
            return Results.Ok(result.Value);
        }).RequireAuthorization(Roles.Member);
        
        ingredients.MapDelete("/{id}", async (string id, IngredientService service) =>
        {
            var result = await service.DeleteIngredient(id);
            if (result.IsFailed)
            {
                return Results.BadRequest(result.Errors);
            }
            return Results.Ok();
        }).RequireAuthorization(Roles.Member);
        
        ingredients.MapPut("/", async ([FromBody]IngredientDto data, IngredientService service) =>
        {
            var result = await service.EditIngredient(data);
            if (result.IsFailed)
            {
                return Results.BadRequest(result.Errors);
            }
            return Results.Ok();
        }).RequireAuthorization(Roles.Member);
        
        ingredients.MapPost("/", async ([FromBody]IngredientCreateDto data, IngredientService service) =>
        {
            var result = await service.CreateIngredient(data);
            if (result.IsFailed)
            {
                return Results.BadRequest(result.Errors);
            }
            return Results.Created();
        }).RequireAuthorization(Roles.Member);
        

        #endregion

        #region Meals

        var meal = app.MapGroup("/meals").WithOpenApi().WithTags("Meals");

        meal.MapPut("/{id}", async ([FromBody] MealEditDto mealData, MealService service) =>
        {
            var result = await service.EditMeal(mealData);
            if (result.IsFailed)
            {
                return Results.BadRequest(result.Errors);
            }
            return Results.Ok();
        }).RequireAuthorization(Roles.Member);
        
        meal.MapGet("/{id}", async (string id, MealService service) =>
        {
            var result = await service.GetMealById(id);
            if (result.IsFailed)
            {
                return Results.BadRequest(result.Errors);
            }

            return Results.Ok(result.Value);
        }).RequireAuthorization(Roles.Member);
        
        meal.MapDelete("/{id}", async (string id, MealService service) =>
        {
            var result = await service.DeleteMeal(id);
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
        
        // get mealplan
        mealplanner.MapGet("/{id}", async (string mealPlanId, MealPlanService service) =>
        {
            var result = await service.GetMealplanById(mealPlanId);
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
        mealplanner.MapDelete("/{id}", async (string id, MealPlanService service) =>
        {
            var result = await service.DeleteMealPlan(id);
            if (result.IsFailed)
            {
                return Results.BadRequest(result.Errors);
            }
            return Results.Ok();
        }).RequireAuthorization(Roles.Member);
        
        // edit mealplan

        mealplanner.MapPut("/{id}", async ([FromBody]MealPlanEditDto data, MealPlanService service) =>
        {
            var result = await service.EditMealPlan(data);
            if (result.IsFailed)
            {
                return Results.BadRequest(result.Errors);
            }
            return Results.Ok();
        }).RequireAuthorization(Roles.Member);
        
        // clear mealplan
        
        mealplanner.MapDelete("/clear/{id}", async (string id, MealPlanService service) =>
        {
            var result = await service.ClearMealPlan(id);
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
            var result = await service.AutoMealPlan(data, householdId);
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