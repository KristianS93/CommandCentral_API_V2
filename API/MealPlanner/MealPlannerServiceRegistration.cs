using API.MealPlanner.Services;

namespace API.MealPlanner;

public static class MealPlannerServiceRegistration
{
    public static IServiceCollection AddMealPlannerServices(this IServiceCollection services)
    {
        services.AddTransient<MealPlannerService>();
        services.AddTransient<IngredientService>();
        services.AddTransient<MealService>();
        
        return services;
    }
}