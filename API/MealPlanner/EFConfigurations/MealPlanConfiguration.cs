using API.Household.Models;
using API.MealPlanner.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.MealPlanner.EFConfigurations;

public class MealPlanConfiguration : IEntityTypeConfiguration<MealPlanModel>
{
    public void Configure(EntityTypeBuilder<MealPlanModel> builder)
    {
        builder.HasKey(key => key.MealPlanId);

        builder.HasAlternateKey(key => new { key.HouseholdId, key.Week, key.Year });

        builder.HasOne<HouseholdModel>(obj => obj.Household)
            .WithMany()
            .HasForeignKey(key => key.HouseholdId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany<MealsInPlan>(obj => obj.Meals)
            .WithOne(obj => obj.MealPlan)
            .HasForeignKey(key => key.MealPlanId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(e => e.CreatedAt)
            .HasColumnType("timestamp");
        builder.Property(e => e.LastModified)
            .HasColumnType("timestamp");
    }
}