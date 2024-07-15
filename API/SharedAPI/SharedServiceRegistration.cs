using API.Identity;
using API.SharedAPI.Persistence;
using Microsoft.EntityFrameworkCore;

namespace API.SharedAPI;

public static class SharedServiceRegistration
{
    public static IServiceCollection AddSharedServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Connection string
        var connectionString = configuration.GetConnectionString("Postgres");
        ArgumentNullException.ThrowIfNullOrEmpty(connectionString);
        services.AddDbContext<ApiDbContext>(options => { options.UseNpgsql(connectionString).UseLowerCaseNamingConvention(); });


        return services;
    }
}