using API.MealPlanner.Models;
using API.SharedAPI.Persistence;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API.MealPlanner.Services;

public class IngredientService
{
    private readonly ApiDbContext _context;
    public IngredientService(ApiDbContext context)
    {
        _context = context;
    }

    public async Task<Result> EditIngredient(IngredientDto data, string householdId)
    {
        if (householdId.IsNullOrEmpty())
        {
            return Result.Fail("Missing id");
        }

        var meal = await _context.Meals
            .Include(i => i.Ingredients)
            .FirstOrDefaultAsync(k => k.MealId == data.MealId && k.HouseholdId == householdId);

        if (meal is null)
        {
            return Result.Fail("Error finding associated meal");
        }
        var ingredient = meal.Ingredients!.FirstOrDefault(i => i.IngredientId == data.IngredientId);
        if (ingredient is null)
        {
            return Result.Fail("Error finding ingredient");
        }

        ingredient.Name = data.Name;
        ingredient.Amount = data.Amount;
        await _context.SaveChangesAsync();
        return Result.Ok();
    }
    
    public async Task<Result> DeleteIngredient(string id, string mealId, string householdId)
    {
        if (id.IsNullOrEmpty() || mealId.IsNullOrEmpty() || householdId.IsNullOrEmpty())
        {
            return Result.Fail("No id provided");
        }
        
        var meal = await _context.Meals
            .Include(i => i.Ingredients)
            .FirstOrDefaultAsync(k => k.MealId == mealId && k.HouseholdId == householdId);
        if (meal is null)
        {
            return Result.Fail("Error finding meal");
        }
        
        var ingredient = meal.Ingredients!.FirstOrDefault(i => i.IngredientId == id);
        if (ingredient is null)
        {
            return Result.Fail("Error finding ingredient");
        }
        _context.Ingredients.Remove(ingredient);
        await _context.SaveChangesAsync();
        return Result.Ok();
    }
    
    public async Task<Result> CreateIngredient(IngredientCreateDto ingredientData, string householdId)
    {
        if (ingredientData.MealId.IsNullOrEmpty() || ingredientData.Name.IsNullOrEmpty())
        {
            return Result.Fail("Missing information");
        }

        var mealExists = _context.Meals.Count(m => m.MealId == ingredientData.MealId && m.HouseholdId == householdId);
        if (mealExists == 0)
        {
            return Result.Fail("Missing meal");
        }
        var newIngredient = new IngredientModel
        {
            MealId = ingredientData.MealId,
            Name = ingredientData.Name,
            Amount = ingredientData.Amount
        };
        await _context.Ingredients.AddAsync(newIngredient);
        await _context.SaveChangesAsync();
        
        return Result.Ok();
    }

    public async Task<Result<IngredientDto>> GetIngredient(string id, string mealId, string householdId)
    {
        if (id.IsNullOrEmpty() || mealId.IsNullOrEmpty() || householdId.IsNullOrEmpty())
        {
            return Result.Fail("No id provided");
        }

        var meal = await _context.Meals
            .Include(i => i.Ingredients)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.MealId == mealId && m.HouseholdId == householdId);
        if (meal is null)
        {
            return Result.Fail("Missing meal");
        }

        var ingredient = meal.Ingredients!.FirstOrDefault(i => i.IngredientId == id);
        if (ingredient is null)
        {
            return Result.Fail("Error retrieving ingredient");
        }

        var dto = new IngredientDto(ingredient.IngredientId, ingredient.MealId, ingredient.Name, ingredient.Amount);
        return dto;
    }
    
    
    
    
    
}