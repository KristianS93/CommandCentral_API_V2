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
    
    public async Task<Result<List<MealPlansDto>>> GetMealPlans(string householdId)
    {
        if (householdId.IsNullOrEmpty())
        {
            return Result.Fail("Missing household id");
        }

        var mealPlans = await _context.MealPlans
            .Where(hh => hh.HouseholdId == householdId)
            .Select(mp => new MealPlansDto(
                mp.MealPlanId,
                mp.WeekNo
                ))
            .ToListAsync();

        return Result.Ok(mealPlans);
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
    }

    public async Task<Result> ClearMealPlan(string id)
    {
        if (id.IsNullOrEmpty())
        {
            return Result.Fail("Id missing");
        }

        var mealsInplan = await _context.MealsInPlans.Where(obj => obj.MealPlanId == id).ToListAsync();
        if (!mealsInplan.IsNullOrEmpty())
        {
            _context.MealsInPlans.RemoveRange(mealsInplan);
            await _context.SaveChangesAsync();
        }
        
        return Result.Ok();
    }

    public async Task<Result> TransferMealPlan(string id, string householdId)
    {
        if (id.IsNullOrEmpty() || householdId.IsNullOrEmpty())
        {
            return Result.Fail("Missing id");
        }

        var groceryList = await _context.GroceryLists
            .Include(i => i.Items)
            .FirstOrDefaultAsync(hh => hh.HousehouldId == householdId);
        if (groceryList is null)
        {
            return Result.Fail("Error loading grocerylist");
        }
        
        var groceryItems = await _context.MealsInPlans
            .Where(m => m.MealPlanId == id)
            .SelectMany(m => m.Meal!.Ingredients!.Select(n => new GroceryItemModel
            {
                Name = n.Name,
                Amount = n.Amount,
                GroceryListId = groceryList.GroceryListId
            })).ToListAsync();
        
        // this works items are succesfully converted, have to add the grocerylist id for this to work. 
        groceryList.Items!.AddRange(groceryItems);
        await _context.SaveChangesAsync();
        return Result.Ok();
    }

    public async Task<Result> CreateMealPlan(MealPlanCreateDto data, string householdId)
    {
        if (householdId.IsNullOrEmpty())
        {
            return Result.Fail("Missing household");
        }
        
        var mealPlan = new MealPlanModel
        {
            HouseholdId = householdId,
        };
        if (data.MealIds is not null)
        {
            var meals = data.MealIds.Select(n => new MealsInPlan
            {
                MealPlanId = mealPlan.MealPlanId,
                MealId = n.MealId,
            }).ToList();
            
            Console.WriteLine("Meals: " + meals.Count);
            await _context.MealsInPlans.AddRangeAsync(meals);
        }
        
        await _context.MealPlans.AddAsync(mealPlan);
        await _context.SaveChangesAsync();
        return Result.Ok();
    }

    public async Task<Result> AutoMealPlan(AutoMealPlanDto data, string householdId)
    {
        if (data.MealPlanId.IsNullOrEmpty() || householdId.IsNullOrEmpty())
        {
            return Result.Fail("Missing id");
        }

        var mealplan = await _context.MealPlans.FindAsync(data.MealPlanId);
        if (mealplan is null)
        {
            return Result.Fail("Error finding mealplan");
        }

        if (data.MealsToAdd == 0)
        {
            return Result.Ok();
        }

        if (data.MealsToAdd > 7)
        {
            return Result.Fail("Pick between 1 and 7 meals");
        }
        
        var meals = await _context.Meals.Where(obj => obj.HouseholdId == householdId).ToListAsync();
        if (meals.Count == 0)
        {
            return Result.Fail("No meals to select from!");
        }
        Random rnd = new Random();
        var itemsToadd = new List<MealsInPlan>();
        for (int i = 0; i < data.MealsToAdd; i++)
        {
            var item = meals[rnd.Next(0, meals.Count)];
            itemsToadd.Add(new MealsInPlan
            {
                MealPlanId = data.MealPlanId,
                MealId = item.MealId,
            });
        }

        await _context.MealsInPlans.AddRangeAsync(itemsToadd);
        await _context.SaveChangesAsync();
        return Result.Ok();
    }

    public async Task<Result> EditMealPlan(MealPlanEditDto data)
    {
        if (data.MealPlanid.IsNullOrEmpty())
        {
            return Result.Fail("Missing id");
        }

        var mealplan = await _context.MealPlans.Include(obj => obj.Meals).FirstOrDefaultAsync(key => key.MealPlanId == data.MealPlanid);
        if (mealplan is null)
        {
            return Result.Fail("Failure getting plan");
        }

        if (data.MealIds is not null)
        {
            var dict = data.MealIds.ToDictionary(e => e.MealId);
            if (mealplan.Meals is not null)
            {
                var meals = mealplan.Meals.ToList();
                meals.RemoveAll(obj =>
                {
                    if (dict.ContainsKey(obj.MealId))
                    {
                        dict.Remove(obj.MealId);
                        return false;
                    }
                    
                    return true;
                });
                if (dict.Count > 0)
                {
                    foreach (var entry in dict)
                    {
                        meals.Add(new MealsInPlan
                        {
                            MealPlanId = data.MealPlanid,
                            MealId = entry.Key
                        });
                    }
                }
                mealplan.Meals = meals;
            }

            mealplan.WeekNo = data.Week;
        }

        await _context.SaveChangesAsync();
        return Result.Ok();
    }
}
