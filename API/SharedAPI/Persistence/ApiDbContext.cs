using API.Household.EFConfigurations;
using API.Household.Models;
using API.identity.Models;
using API.SharedAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.SharedAPI.Persistence;

public class ApiDbContext : IdentityDbContext<CCAIdentity>
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<HouseholdModel> Households { get; set; }
    public DbSet<HouseholdUsersModel> HouseholdUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfiguration(new HouseholdConfiguration()).ApplyConfiguration(new HouseholdUserRelationConfiguration());
        // builder.ApplyConfigurationsFromAssembly(typeof(IdentityDbContext).Assembly);
    }
    
    public async Task<int> SaveChangesAsync()
    {
        foreach (var entry in base.ChangeTracker.Entries<BaseEntity>().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
        {
            entry.Entity.LastModified = DateTime.UtcNow;
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
            }
        }

        return await base.SaveChangesAsync();
    }
}