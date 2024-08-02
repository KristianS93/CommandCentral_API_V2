using API.GroceryList.Models;
using API.MealPlanner.Models;
using API.SharedAPI.Persistence;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API.MealPlanner.Services;

public class MealPlanService
{
    private readonly ApiDbContext _context;
    public MealPlanService(ApiDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<MealPlanDto>>> GetMealplans(string householdId)
    {
        await Task.Delay(1);
        return new Result<List<MealPlanDto>>();
    }

    public async Task<Result> DeleteMealPlan(string id)
    {
        if (id.IsNullOrEmpty())
        {
            return Result.Fail("Missing id");
        }

        var mealplan = await _context.MealPlans.FindAsync(id);
        if (mealplan is null)
        {
            return Result.Fail("Error retrieving mealplan");
        }
        _context.MealPlans.Remove(mealplan);
        await _context.SaveChangesAsync();
        return Result.Ok();
    }
    public async Task<Result<MealPlanDto>> GetMealplanById(string id)
    {
        
        if (string.IsNullOrEmpty(id))
        {
            return Result.Fail("No id provided");
        }

        var mealplanDto = await _context.MealPlans
            .Where(mp => mp.MealPlanId == id)
            .Select(mp => new MealPlanDto(
                mp.MealPlanId,
                mp.WeekNo,
                mp.Meals!.Select(mip => new MealInfoDto(
                    mip.Meal!.MealId,
                    mip.Meal.Name
                )).ToList()
            ))
            .FirstOrDefaultAsync();

        if (mealplanDto == null)
        {
            return Result.Fail("Error loading meal plan");
        }

        return Result.Ok(mealplanDto);
        // if (id.IsNullOrEmpty())
        // {
        //     return Result.Fail("No id provided");
        // }
        //
        // var mealplanModel = await _context.MealPlans.FindAsync(id);
        // if (mealplanModel is null)
        // {
        //     return Result.Fail("Error loading meal plan");
        // }
        // var meals = await _context.MealsInPlans.Where(key => key.MealPlanId == id).ToListAsync();
        //
        // var mealsDto = new List<MealInfoDto>();
        // foreach (var meal in meals)
        // {
        //     var res = await _context.Meals.FindAsync(meal.MealId);
        //     mealsDto.Add(new MealInfoDto(res!.MealId, res.Name));
        // }
        //
        // var result = new MealPlanDto(mealplanModel.MealPlanId, mealplanModel.WeekNo, mealsDto);
        //
        // return result;
    }

    public async Task<Result> ClearMealPlan(string id)
    {
        if (id.IsNullOrEmpty())
        {
            return Result.Fail("Id missing");
        }

        var mealsInplan = await _context.MealsInPlans.Where(obj => obj.MealPlanId == id).ToListAsync();
        Console.WriteLine("Count " + mealsInplan.Count);
        if (mealsInplan.Count > 0)
        {
            Console.WriteLine("No meals");
            _context.MealsInPlans.RemoveRange(mealsInplan);
            await _context.SaveChangesAsync();
        }
        
        return Result.Ok();
    }

    public async Task<Result> TransferMealPlan(string id)
    {
        if (id.IsNullOrEmpty())
        {
            return Result.Fail("Missing id");
        }

        var groceryItems = await _context.MealsInPlans
            .Where(m => m.MealPlanId == id)
            .SelectMany(m => m.Meal!.Ingredients!.Select(n => new IngredientModel
            {
                Name = n.Name,
                Amount = n.Amount,
            })).ToListAsync();
        
        // this works items are succesfully converted, have to add the grocerylist id for this to work. 
        Console.WriteLine("Items " + groceryItems.Count);
        foreach (var item in groceryItems)
        {
            Console.WriteLine(item.Name);
        }

        return Result.Ok();

    }
}
