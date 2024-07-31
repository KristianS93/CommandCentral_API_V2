namespace API.MealPlanner;

public static class MealPlannerEndpoints
{
    public static WebApplication AddMealPlannerEndpoints(this WebApplication app)
    {
        var mealplanner = app.MapGroup("/mealplanner").WithOpenApi().WithTags("Mealplanner");

        #region Ingredients

        

        #endregion

        #region Meals

        

        #endregion

        #region MealPlan

        

        #endregion
        
        
        
        return app;
    }
}