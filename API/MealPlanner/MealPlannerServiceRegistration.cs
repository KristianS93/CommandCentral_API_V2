namespace API.MealPlanner;

public static class MealPlannerServiceRegistration
{
    public static IServiceCollection AddMealPlannerServices(this IServiceCollection services)
    {
        services.AddTransient<MealPlannerService>();
        
        return services;
    }
}