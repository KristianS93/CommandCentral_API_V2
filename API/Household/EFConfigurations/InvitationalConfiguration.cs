using API.Household.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Household.EFConfigurations;

public class InvitationalConfiguration : IEntityTypeConfiguration<InvitationModel>
{
    public void Configure(EntityTypeBuilder<InvitationModel> builder)
    {
        builder.HasKey(key => key.InvitationId);

        builder.HasOne(o => o.Household)
            .WithMany()
            .HasForeignKey(key => key.HouseholdId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(e => e.CreatedAt)
            .HasColumnType("timestamp");
        builder.Property(e => e.LastModified)
            .HasColumnType("timestamp");
    }
}