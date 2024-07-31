using API.MealPlanner.Models;
using API.MealPlanner.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.MealPlanner;

public static class MealPlannerEndpoints
{
    public static WebApplication AddMealPlannerEndpoints(this WebApplication app)
    {
        var mealplanner = app.MapGroup("/mealplanner").WithOpenApi().WithTags("Mealplanner");

        #region Ingredients

        var ingredients = mealplanner.MapGroup("/ingredients").WithOpenApi().WithTags("Ingredients");
        ingredients.MapGet("/{id}", async (string id) =>
        {
            await Task.Delay(1);
            return Results.Ok();
        });

        ingredients.MapDelete("/{id}", async (string id) =>
        {
            await Task.Delay(1);
            return Results.Ok();
        });
        
        ingredients.MapPut("/", async () =>
        {
            await Task.Delay(1);
            return Results.Ok(); 
        });

        ingredients.MapPost("/", async () =>
        {
            await Task.Delay(1);
            return Results.Ok();
        });
        

        #endregion

        #region Meals

        var meal = mealplanner.MapGroup("/meals").WithOpenApi().WithTags("Meals");

        meal.MapPut("/{id}", async ([FromBody] MealEditDto mealData, MealService service) =>
        {
            var result = await service.EditMeal(mealData);
            if (result.IsFailed)
            {
                return Results.BadRequest(result.Errors);
            }
            return Results.Ok();
        });
        
        meal.MapGet("/{id}", async (string id, MealService service) =>
        {
            var result = await service.GetMealById(id);
            if (result.IsFailed)
            {
                return Results.BadRequest(result.Errors);
            }

            return Results.Ok(result.Value);
        });
        
        meal.MapDelete("/{id}", async (string id, MealService service) =>
        {
            var result = await service.DeleteMeal(id);
            if (result.IsFailed)
            {
                return Results.BadRequest(result.Errors);
            }

            return Results.Ok();
        });
        
        meal.MapPost("/", async ([FromBody]MealCreateDto mealData, MealService service) =>
        {
            var result = await service.CreateMeal(mealData);
            if (result.IsFailed)
            {
                return Results.BadRequest(result.Errors);
            }

            return Results.Created();
        });

        #endregion

        #region MealPlan

        

        #endregion
        
        
        
        return app;
    }
}