using API.Identity;
using API.SharedAPI.Persistence;
using Microsoft.EntityFrameworkCore;

namespace API.SharedAPI;

public static class SharedAppRegistration
{
    public static async Task<WebApplication> AddSharedApp(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        
        // ensure migration
        var dbContext = scope.ServiceProvider.GetRequiredService<ApiDbContext>();
        await dbContext.Database.MigrateAsync();
        
        return app;
    }
}