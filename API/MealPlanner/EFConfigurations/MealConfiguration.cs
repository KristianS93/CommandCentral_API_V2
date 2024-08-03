using API.Household.Models;
using API.MealPlanner.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.MealPlanner.EFConfigurations;

public class MealConfiguration : IEntityTypeConfiguration<MealModel>
{
    public void Configure(EntityTypeBuilder<MealModel> builder)
    {
        builder.HasKey(key => key.MealId);

        builder.HasMany<IngredientModel>(obj => obj.Ingredients)
            .WithOne(ingredient => ingredient.Meal)
            .HasForeignKey(key => key.MealId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<HouseholdModel>(obj => obj.Household)
            .WithMany()
            .HasForeignKey(key => key.HouseholdId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(e => e.CreatedAt)
            .HasColumnType("timestamp");
        builder.Property(e => e.LastModified)
            .HasColumnType("timestamp");
    }
}