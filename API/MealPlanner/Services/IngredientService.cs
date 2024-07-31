using API.SharedAPI.Persistence;

namespace API.MealPlanner.Services;

public class IngredientService
{
    private readonly ApiDbContext _context;
    public IngredientService(ApiDbContext context)
    {
        _context = context;
    }
    
    
    
}