using API.GroceryList.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.GroceryList.EFConfigurations;

public class GroceryListConfiguration : IEntityTypeConfiguration<GroceryListModel>
{
    public void Configure(EntityTypeBuilder<GroceryListModel> builder)
    {

        builder.HasKey(key => key.GroceryListId);
        builder.HasOne(h => h.Household)
            .WithMany()
            .HasForeignKey(key => key.HousehouldId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(e => e.CreatedAt)
            .HasColumnType("timestamp");
        builder.Property(e => e.LastModified)
            .HasColumnType("timestamp");
    }
}