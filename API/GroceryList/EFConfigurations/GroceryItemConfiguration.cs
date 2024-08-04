using API.GroceryList.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.GroceryList.EFConfigurations;

public class GroceryItemConfiguration : IEntityTypeConfiguration<GroceryItemModel>
{
    public void Configure(EntityTypeBuilder<GroceryItemModel> builder)
    {
        builder.HasKey(key => key.ItemId);
        builder.HasOne<GroceryListModel>(g => g.GroceryList)
            .WithMany(obj => obj.Items)
            .HasForeignKey(key => key.GroceryListId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(e => e.CreatedAt)
            .HasColumnType("timestamp");
        builder.Property(e => e.LastModified)
            .HasColumnType("timestamp");
    }
}