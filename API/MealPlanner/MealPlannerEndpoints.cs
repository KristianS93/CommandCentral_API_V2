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

        meal.MapPost("/", async ([FromBody]MealCreateDto meal, MealService service) =>
        {
            var result = await service.CreateMeal(meal);
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