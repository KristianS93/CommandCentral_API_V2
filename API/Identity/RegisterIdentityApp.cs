using API.Identity;
using API.identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.identity;

public static class RegisterIdentityApp
{
    public static async Task<WebApplication> AddIdentityApp(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<CCAIdentity>>();
        
        // ensure migration
        var dbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
        await dbContext.Database.MigrateAsync();

        

        string[] roleNames = { Roles.Admin, Roles.Owner, Roles.Member };

        foreach (var role in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
}