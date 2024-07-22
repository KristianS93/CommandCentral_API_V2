using API.GroceryList.Hubs;

namespace API.GroceryList;

public static class GroceryListServiceRegistration
{
    public static IServiceCollection AddGroceryListServices(this IServiceCollection services)
    {
        services.AddSignalR();
        services.AddSingleton<SharedConnections>();
        services.AddScoped<GroceryListService>();
        return services;
    }
}