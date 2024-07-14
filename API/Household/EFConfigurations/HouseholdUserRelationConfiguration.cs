using API.Household.Models;
using API.identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Household.EFConfigurations;

public class HouseholdUserRelationConfiguration : IEntityTypeConfiguration<HouseholdUsersModel>
{
    public void Configure(EntityTypeBuilder<HouseholdUsersModel> builder)
    {
        builder.HasKey(key => new { key.HouseholdId, key.UserId });

        // Define relationships
        builder.HasOne(hu => hu.Household)
            .WithMany(h => h.HouseholdUsers)
            .HasForeignKey(hu => hu.HouseholdId)
            .OnDelete(DeleteBehavior.Cascade); // Cascade delete if desired

        builder.HasOne(hu => hu.User)
            .WithMany() // No navigation property in CCAIdentity pointing back to HouseholdUsersModel
            .HasForeignKey(hu => hu.UserId)
            .OnDelete(DeleteBehavior.Cascade); // Cascade delete if desired
    }
}