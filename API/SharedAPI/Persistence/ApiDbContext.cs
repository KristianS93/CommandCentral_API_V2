using API.Household.Models;
using Microsoft.EntityFrameworkCore;

namespace API.SharedAPI.Persistence;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
    {
        
    }

    public DbSet<HouseholdModel> Households { get; set; }
    public DbSet<HouseholdUsersModel> HouseholdUsers { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApiDbContext).Assembly);

    }
}