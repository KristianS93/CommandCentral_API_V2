// using API.Household;
// using API.Household.Models;
// using API.SharedAPI.Models;
// using Microsoft.EntityFrameworkCore;
//
// namespace API.SharedAPI.Persistence;
//
// public class WillDelete : DbContext
// {
//     public WillDelete(DbContextOptions<WillDelete> options) : base(options)
//     {
//         
//     }
//
//     public DbSet<HouseholdModel> Households { get; set; }
//     public DbSet<HouseholdUserRelationModel> HouseholdUserRelation { get; set; }
//
//     protected override void OnModelCreating(ModelBuilder modelBuilder)
//     {
//         base.OnModelCreating(modelBuilder);
//         modelBuilder.ApplyConfigurationsFromAssembly(typeof(WillDelete).Assembly);
//     }
//
//     public async Task<int> SaveChangesAsync()
//     {
//         foreach (var entry in base.ChangeTracker.Entries<BaseEntity>().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
//         {
//             entry.Entity.LastModified = DateTime.UtcNow;
//             if (entry.State == EntityState.Added)
//             {
//                 entry.Entity.CreatedAt = DateTime.UtcNow;
//             }
//         }
//
//         return await base.SaveChangesAsync();
//     }
// }