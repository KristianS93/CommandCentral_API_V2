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

    public async Task<Result<List<MealIdNameDto>>> GetMeals(string householdId)
    {
        if (householdId.IsNullOrEmpty())
        {
            return Result.Fail("Missing id");
        }

        var meals = await _context.Meals
            .Where(m => m.HouseholdId == householdId)
            .Select(n => new MealIdNameDto(n.MealId, n.Name))
            .AsNoTracking()
            .ToListAsync();
        return meals;
    }
    public async Task<Result> EditMeal(MealEditDto mealData, string householdId)
    {
        if (householdId.IsNullOrEmpty())
        {
            return Result.Fail("Missing id");
        }
        var meal = await _context.Meals
            .FirstOrDefaultAsync(m => m.MealId == mealData.MealId && m.HouseholdId == householdId);
        if (meal is null)
        {
            return Result.Fail("Could not find meal");
        }

        meal.Name = mealData.Name;
        meal.Description = mealData.Description;
        await _context.SaveChangesAsync();
        return Result.Ok();
    }
    public async Task<Result<MealDto>> GetMealById(string mealId, string householdId)
    {
        if (mealId.IsNullOrEmpty() || householdId.IsNullOrEmpty())
        {
            return Result.Fail("Missing meal id");
        }

        var mealData = await _context.Meals
            .Include(i => i.Ingredients)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.MealId == mealId && m.HouseholdId == householdId);
        if (mealData is null)
        {
            return Result.Fail("Meal does not exist");
        }

        if (mealData.Ingredients is null)
        {
            mealData.Ingredients = new List<IngredientModel>();
        }
        
        var ingredients = mealData.Ingredients.Select(obj => new MealIngredientDto(obj.IngredientId, obj.Name, obj.Amount)).ToList();
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

    public async Task<Result> DeleteMeal(string mealId, string householdId)
    {
        if (mealId.IsNullOrEmpty() || householdId.IsNullOrEmpty())
        {
            return Result.Fail("Missing id");
        }
        var meal = await _context.Meals.FirstOrDefaultAsync(m => m.MealId == mealId && m.HouseholdId == householdId);
        if (meal is null)
        {
            return Result.Fail("Error deleting meal");
        }

        _context.Meals.Remove(meal);
        await _context.SaveChangesAsync();
        return Result.Ok();
    }
    
}