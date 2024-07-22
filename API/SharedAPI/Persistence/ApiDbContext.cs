using API.GroceryList.Models;
using API.Household.Models;
using API.SharedAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace API.SharedAPI.Persistence;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
    {
        
    }

    public DbSet<HouseholdModel> Households { get; set; }
    public DbSet<HouseholdUsersModel> HouseholdUsers { get; set; }

    public DbSet<InvitationModel> Invitations { get; set; }

    public DbSet<GroceryListModel> GroceryLists { get; set; }

    public DbSet<GroceryItemModel> GroceryListItems { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApiDbContext).Assembly);
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