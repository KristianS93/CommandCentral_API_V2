namespace API.Household;

public static class HouseholdServiceRegistration
{
    public static IServiceCollection AddHouseholdServices(this IServiceCollection services)
    {
        services.AddTransient<HouseholdService>();
        return services;
    }
}