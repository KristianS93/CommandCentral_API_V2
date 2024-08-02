using API.MealPlanner.Models;
using API.SharedAPI.Persistence;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API.MealPlanner.Services;

public class MealService
{
    private readonly ApiDbContext _context;
    public MealService(ApiDbContext context)
    {
        _context = context;
    }

    public async Task<Result> EditMeal(MealEditDto mealData)
    {
        var meal = await _context.Meals.FindAsync(mealData.MealId);
        if (meal is null)
        {
            return Result.Fail("Could not find meal");
        }

        meal.Name = mealData.Name;
        meal.Description = mealData.Description;
        await _context.SaveChangesAsync();
        return Result.Ok();
    }
    public async Task<Result<MealDto>> GetMealById(string mealId)
    {
        if (mealId.IsNullOrEmpty())
        {
            return Result.Fail("Missing meal id");
        }

        var mealData = await _context.Meals
            .Include(i => i.Ingredients)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.MealId == mealId);
        if (mealData is null)
        {
            return Result.Fail("Meal does not exist");
        }

        if (mealData.Ingredients is null)
        {
            mealData.Ingredients = new List<IngredientModel>();
        }
        
        var ingredients = mealData.Ingredients.Select(obj => new IngredientDto(obj.IngredientId, obj.Name, obj.Amount)).ToList();
        var meal = new MealDto(mealData.MealId, mealData.Name, mealData.Description, ingredients);
        return meal;
    }
    public async Task<Result> CreateMeal(MealCreateDto mealData, string householdId)
    {
        if (mealData.Name.IsNullOrEmpty())
        {
            Result.Fail("Missing name");
        }

        var meal = new MealModel
        {
            Name = mealData.Name,
            Description = mealData.Description ?? "",
            HouseholdId = householdId,
        };
        
        await _context.Meals.AddAsync(meal);
        await _context.SaveChangesAsync();

        return Result.Ok();
    }

    public async Task<Result> DeleteMeal(string mealId)
    {
        var meal = await _context.Meals.FindAsync(mealId);
        if (meal is null)
        {
            return Result.Fail("Error deleting meal");
        }

        _context.Meals.Remove(meal);
        await _context.SaveChangesAsync();
        return Result.Ok();
    }
    
}