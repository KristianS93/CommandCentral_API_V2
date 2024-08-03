using API.MealPlanner.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.MealPlanner.EFConfigurations;

public class MealsInPlanConfiguration : IEntityTypeConfiguration<MealsInPlan>
{
    public void Configure(EntityTypeBuilder<MealsInPlan> builder)
    {
        builder.HasKey(key => key.MealsInPlanId);

        builder.HasOne<MealModel>(obj => obj.Meal)
            .WithMany()
            .HasForeignKey(obj => obj.MealId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<MealPlanModel>(obj => obj.MealPlan)
            .WithMany(mp => mp.Meals)
            .HasForeignKey(obj => obj.MealPlanId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}