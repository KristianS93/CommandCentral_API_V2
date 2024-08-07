using API.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.SharedAPI.Persistence;

public static class RawTablesRegister
{
    public static async Task<WebApplication> AddRawTables(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<ApiDbContext>();
        
        // household user relation
        var sql = @"
            DO $$
            BEGIN
            IF (SELECT COUNT(*) FROM ""HouseholdUsers"") = 0 THEN
                DROP TABLE ""HouseholdUsers"";
            END IF;
            CREATE TABLE IF NOT EXISTS ""HouseholdUsers"" (
                HouseholdId VARCHAR(100) NOT NULL,
                UserId VARCHAR(100) NOT NULL,
                Role VARCHAR(100) NOT NULL,
                PRIMARY KEY (UserId),
                FOREIGN KEY (HouseholdId) REFERENCES ""households""(""householdid"") ON DELETE CASCADE,
                FOREIGN KEY (UserId) REFERENCES ""AspNetUsers""(""id"") ON DELETE CASCADE                            
            );
            END
            $$;
        ";
        await dbContext.Database.ExecuteSqlRawAsync(sql);
        return app;
    }
}