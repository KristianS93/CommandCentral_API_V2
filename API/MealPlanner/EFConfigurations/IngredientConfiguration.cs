using API.MealPlanner.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.MealPlanner.EFConfigurations;

public class IngredientConfiguration : IEntityTypeConfiguration<IngredientModel>
{
    public void Configure(EntityTypeBuilder<IngredientModel> builder)
    {
        builder.HasKey(key => key.IngredientId);
        builder.HasOne<MealModel>(key => key.Meal)
            .WithMany(obj => obj.Ingredients)
            .HasForeignKey(key => key.MealId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(e => e.CreatedAt)
            .HasColumnType("timestamp");
        builder.Property(e => e.LastModified)
            .HasColumnType("timestamp");
    }
}