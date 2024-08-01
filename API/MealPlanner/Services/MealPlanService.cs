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
}
