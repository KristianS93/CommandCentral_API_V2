using API.MealPlanner.Models;
using API.SharedAPI.Persistence;
using FluentResults;
using Microsoft.IdentityModel.Tokens;

namespace API.MealPlanner.Services;

public class MealService
{
    private readonly ApiDbContext _context;
    public MealService(ApiDbContext context)
    {
        _context = context;
    }

    public async Task<Result> CreateMeal(MealCreateDto mealData)
    {
        if (mealData.Name.IsNullOrEmpty())
        {
            Result.Fail("Missing name");
        }

        var meal = new MealModel
        {
            Name = mealData.Name,
            Description = mealData.Description ?? "",
        };
        
        await _context.Meals.AddAsync(meal);
        await _context.SaveChangesAsync();

        return Result.Ok();
    }
}