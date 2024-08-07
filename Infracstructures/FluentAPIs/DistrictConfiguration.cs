using Domain.Models.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infracstructures.FluentAPIs;

public class DistrictConfiguration : IEntityTypeConfiguration<District>
{
    public void Configure(EntityTypeBuilder<District> builder)
    {
        builder.ToTable("District");

        //Id
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasDefaultValueSql("NEWID()");

        //Name
        builder.Property(u => u.Name).IsRequired().HasMaxLength(50);
        builder.HasIndex(x => x.Name).IsUnique();

        //Relation
        builder.HasMany(u => u.Wards).WithOne(u => u.District).HasForeignKey(u => u.DistrictId)
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}