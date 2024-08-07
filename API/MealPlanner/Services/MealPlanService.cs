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

    public async Task<Result> AddMealToPlan(string mealPlanId, string householdId, MealPlanAddMeal data)
    {
        if (mealPlanId.IsNullOrEmpty() || householdId.IsNullOrEmpty())
        {
            return Result.Fail("Missing id");
        }

        var mealExists = _context.Meals.Count(m => m.MealId == data.MealId && m.HouseholdId == householdId);
        if (mealExists == 0)
        {
            return Result.Fail("Error retreiving meal");
        }

        var planExists = _context.MealPlans.Count(p => p.MealPlanId == mealPlanId && p.HouseholdId == householdId);
        if (planExists == 0)
        {
            return Result.Fail("Error retreiving plan");
        }

        var mealInplan = new MealsInPlan
        {
            MealPlanId = mealPlanId,
            MealId = data.MealId,
            MealDay = data.MealDay
        };
        await _context.MealsInPlans.AddAsync(mealInplan);
        await _context.SaveChangesAsync();
        return Result.Ok();
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
                mp.Year,
                mp.Week
                ))
            .ToListAsync();

        return Result.Ok(mealPlans);
    }

    public async Task<Result> DeleteMealPlan(string id, string householdId)
    {
        if (id.IsNullOrEmpty())
        {
            return Result.Fail("Missing id");
        }
        
        // confirm the meal id is associated to this household

        var mealplan = await _context.MealPlans
            .FirstOrDefaultAsync(m => m.MealPlanId == id && m.HouseholdId == householdId);
        if (mealplan is null)
        {
            return Result.Fail("Error retrieving mealplan");
        }
        _context.MealPlans.Remove(mealplan);
        await _context.SaveChangesAsync();
        return Result.Ok();
    }
    public async Task<Result<MealPlanDto>> GetMealplanById(MealPlanGetDto meal, string householdId)
    {
        
        if (string.IsNullOrEmpty(householdId))
        {
            return Result.Fail("No id provided");
        }

        var mealplanDto = await _context.MealPlans
            .Where(mp => mp.Year == meal.Year && mp.Week == meal.Week && mp.HouseholdId == householdId)
            .Select(mp => new MealPlanDto(
                mp.MealPlanId,
                mp.Year,
                mp.Week,
                mp.Meals!.Select(mip => new MealInfoDto(
                    mip.Meal!.MealId,
                    mip.Meal.Name,
                    mip.MealDay
                )).ToList()
            ))
            .FirstOrDefaultAsync();

        if (mealplanDto == null)
        {
            return Result.Fail("Error loading meal plan");
        }

        return Result.Ok(mealplanDto);
    }

    public async Task<Result> ClearMealPlan(string id, string householdId)
    {
        if (id.IsNullOrEmpty() || householdId.IsNullOrEmpty())
        {
            return Result.Fail("Id missing");
        }

        var meals = await _context.MealPlans.Include(obj => obj.Meals)
            .FirstOrDefaultAsync(mp => mp.HouseholdId == householdId && mp.MealPlanId == id);
        if (meals is null)
        {
            return Result.Fail("Could not retrieve meals");
        }

        if (!meals.Meals.IsNullOrEmpty())
        {
            _context.MealsInPlans.RemoveRange(meals.Meals!);
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
            .Include(obj => obj.MealPlan)
            .Where(m => m.MealPlanId == id && m.MealPlan!.HouseholdId == householdId)
            .SelectMany(m => m.Meal!.Ingredients!.Select(
                n => new GroceryItemModel
                {
                    Name = n.Name,
                    Amount = n.Amount,
                    GroceryListId = groceryList.GroceryListId
                }))
            .ToListAsync();
        
        await _context.GroceryListItems.AddRangeAsync(groceryItems);
        await _context.SaveChangesAsync();
        return Result.Ok();
    }

    public async Task<Result> CreateMealPlan(MealPlanCreateDto data, string householdId)
    {
        if (householdId.IsNullOrEmpty())
        {
            return Result.Fail("Missing household");
        }
        
        // check whether this date is already created 

        var oldMealPlan = await _context.MealPlans.FirstOrDefaultAsync(o => o.HouseholdId == householdId && o.Year == data.Year && o.Week == data.Week);
        if (oldMealPlan is not null)
        {
            return Result.Fail("Meal plan already exist.");
        }
        
        var mealPlan = new MealPlanModel
        {
            HouseholdId = householdId,
            Year = data.Year,
            Week = data.Week
        };
        if (data.MealIds is not null)
        {
            var meals = data.MealIds.Select(n => new MealsInPlan
            {
                MealPlanId = mealPlan.MealPlanId,
                MealId = n.MealId,
                MealDay = n.MealDay
            }).ToList();
            
            Console.WriteLine("Meals: " + meals.Count);
            await _context.MealsInPlans.AddRangeAsync(meals);
        }
        
        await _context.MealPlans.AddAsync(mealPlan);
        await _context.SaveChangesAsync();
        return Result.Ok();
    }

    public async Task<Result> GenerateMealPlan(AutoMealPlanDto data, string householdId)
    {
        if (data.MealPlanId.IsNullOrEmpty() || householdId.IsNullOrEmpty())
        {
            return Result.Fail("Missing id");
        }

        var mealplan = await _context.MealPlans
            .FirstOrDefaultAsync(p => p.MealPlanId == data.MealPlanId && p.HouseholdId == householdId);
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

    public async Task<Result> EditMealPlan(MealPlanEditDto data, string householdId)
    {
        if (data.MealPlanid.IsNullOrEmpty() || householdId.IsNullOrEmpty())
        {
            return Result.Fail("Missing id");
        }

        var mealplan = await _context.MealPlans.Include(obj => obj.Meals)
            .FirstOrDefaultAsync(key => key.MealPlanId == data.MealPlanid && key.HouseholdId == householdId);
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
                            MealId = entry.Key,
                            MealDay = entry.Value.MealDay
                        });
                    }
                }
                mealplan.Meals = meals;
            }

            mealplan.Year = data.Year;
            mealplan.Week = data.Week;
        }

        await _context.SaveChangesAsync();
        return Result.Ok();
    }
}
