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
        ingredients.MapGet("/{id}", async (string id, IngredientService service) =>
        {
            var result = await service.GetIngredient(id);
            if (result.IsFailed)
            {
                return Results.BadRequest(result.Errors);
            }
            return Results.Ok(result.Value);
        });
        
        ingredients.MapDelete("/{id}", async (string id, IngredientService service) =>
        {
            var result = await service.DeleteIngredient(id);
            if (result.IsFailed)
            {
                return Results.BadRequest(result.Errors);
            }
            return Results.Ok();
        });
        
        ingredients.MapPut("/", async ([FromBody]IngredientDto data, IngredientService service) =>
        {
            var result = await service.EditIngredient(data);
            if (result.IsFailed)
            {
                return Results.BadRequest(result.Errors);
            }
            return Results.Ok();
        });
        
        ingredients.MapPost("/", async ([FromBody]IngredientCreateDto data, IngredientService service) =>
        {
            var result = await service.CreateIngredient(data);
            if (result.IsFailed)
            {
                return Results.BadRequest(result.Errors);
            }
            return Results.Created();
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
        // get mealplan

        mealplanner.MapGet("/{id}", async (string id, MealPlanService service) =>
        {
            var result = await service.GetMealplanById(id);
            if (result.IsFailed)
            {
                return Results.BadRequest(result.Errors);
            }
            return Results.Ok(result.Value);
        });
        
        // create mealplan

        mealplanner.MapPost("/", async ([FromBody] MealPlanCreateDto data, MealPlanService service) =>
        {
            var result = await service.CreateMealPlan(data);
            if (result.IsFailed)
            {
                return Results.BadRequest(result.Errors);
            }
            return Results.Created();
        });
        
        // delete mealplan
        mealplanner.MapDelete("/{id}", async (string id, MealPlanService service) =>
        {
            var result = await service.DeleteMealPlan(id);
            if (result.IsFailed)
            {
                return Results.BadRequest(result.Errors);
            }
            return Results.Ok();
        });
        
        // edit mealplan

        mealplanner.MapPut("/{id}", async ([FromBody]MealPlanEditDto data, MealPlanService service) =>
        {
            var result = await service.EditMealPlan(data);
            if (result.IsFailed)
            {
                return Results.BadRequest(result.Errors);
            }
            return Results.Ok();
        });
        
        // clear mealplan
        
        mealplanner.MapDelete("/clear/{id}", async (string id, MealPlanService service) =>
        {
            var result = await service.ClearMealPlan(id);
            if (result.IsFailed)
            {
                return Results.BadRequest(result.Errors);
            }
            return Results.Ok();
        });
        
        // auto add x amount of meals from own meal library
        
        // transfer to grocery list.
        mealplanner.MapPut("/groceries/{id}", async (string mealplanId, MealPlanService service) =>
        {
            var result = await service.TransferMealPlan(mealplanId);
            if (result.IsFailed)
            {
                return Results.BadRequest(result.Errors);
            }
            return Results.Ok();
        });
        
        #endregion
        
        return app;
    }
}