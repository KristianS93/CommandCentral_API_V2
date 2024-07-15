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
        builder.ToTable("householdusers");
    }
}