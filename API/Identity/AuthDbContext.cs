using API.identity.Models;
using API.SharedAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Identity;

public class AuthDbContext : IdentityDbContext<CCAIdentity>
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
        
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // builder.ApplyConfiguration(new HouseholdConfiguration()).ApplyConfiguration(new HouseholdUserRelationConfiguration());
        builder.ApplyConfigurationsFromAssembly(typeof(IdentityDbContext).Assembly);
    }
    
    public async Task<int> SaveChangesAsync()
    {
        foreach (var entry in base.ChangeTracker.Entries<BaseEntity>().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
        {
            entry.Entity.LastModified = DateTime.Now;
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.Now;
            }
        }

        return await base.SaveChangesAsync();
    }
}