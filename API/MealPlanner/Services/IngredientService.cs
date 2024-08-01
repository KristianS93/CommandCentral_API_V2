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

    public async Task<Result> EditIngredient(IngredientDto data)
    {
        if (data.IngredientId.IsNullOrEmpty() || data.Name.IsNullOrEmpty())
        {
            return Result.Fail("Missing properties");
        }

        var ingredient = await _context.Ingredients.FindAsync(data.IngredientId);
        if (ingredient is null)
        {
            return Result.Fail("Error finding ingredient");
        }

        ingredient.Name = data.Name;
        ingredient.Amount = data.Amount;
        await _context.SaveChangesAsync();
        return Result.Ok();
    }
    
    public async Task<Result> DeleteIngredient(string id)
    {
        if (id.IsNullOrEmpty())
        {
            return Result.Fail("No id provided");
        }

        var ingredient = await _context.Ingredients.FindAsync(id);
        if (ingredient is null)
        {
            return Result.Fail("Error finding ingredient");
        }
        _context.Ingredients.Remove(ingredient);
        await _context.SaveChangesAsync();
        return Result.Ok();
    }
    
    public async Task<Result> CreateIngredient(IngredientCreateDto ingredientData)
    {
        if (ingredientData.MealId.IsNullOrEmpty() || ingredientData.Name.IsNullOrEmpty())
        {
            return Result.Fail("Missing information");
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

    public async Task<Result<IngredientDto>> GetIngredient(string id)
    {
        if (id.IsNullOrEmpty())
        {
            return Result.Fail("No id provided");
        }

        var ingredientModel = await _context.Ingredients
            .AsNoTracking()
            .SingleOrDefaultAsync(o => o.IngredientId == id);
        if (ingredientModel is null)
        {
            return Result.Fail("Error retrieving ingredient");
        }

        var dto = new IngredientDto(ingredientModel.IngredientId, ingredientModel.Name, ingredientModel.Amount);
        return dto;
    }
    
    
    
    
    
}