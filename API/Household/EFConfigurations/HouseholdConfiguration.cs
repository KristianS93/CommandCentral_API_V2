using API.Household.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Household.EFConfigurations;

public class HouseholdConfiguration : IEntityTypeConfiguration<HouseholdModel>
{
    public void Configure(EntityTypeBuilder<HouseholdModel> builder)
    {
        builder.HasKey(key => key.HouseholdId);
        
        builder.Property(e => e.CreatedAt)
            .HasColumnType("timestamp");
        builder.Property(e => e.LastModified)
            .HasColumnType("timestamp");
    }
}