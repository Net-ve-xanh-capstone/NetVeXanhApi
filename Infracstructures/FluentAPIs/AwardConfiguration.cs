using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infracstructures.FluentAPIs;

public class AwardConfiguration : IEntityTypeConfiguration<Award>
{
    public void Configure(EntityTypeBuilder<Award> builder)
    {
        builder.ToTable("Award");

        //Id
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id)
            .HasDefaultValueSql("NEWID()");

        //CreateTime
        builder.Property(u => u.CreatedTime);

        //CreateBy
        builder.Property(u => u.CreatedBy);

        //UpdateTime
        builder.Property(u => u.UpdatedTime);

        //UpdateBy
        builder.Property(u => u.UpdatedBy);

        //Status
        builder.Property(u => u.Status);

        //Rank
        builder.Property(u => u.Rank);

        //Quantity
        builder.Property(u => u.Quantity);

        //Cash
        builder.Property(u => u.Cash).HasDefaultValue(0.0);

        //Artifact
        builder.Property(u => u.Artifact).HasDefaultValue("");

        //Description
        builder.Property(u => u.Description).HasDefaultValue("");

        //Relation
        //Round
        builder.HasOne(u => u.Round)
            .WithMany(u => u.Award)
            .HasForeignKey(u => u.RoundId).OnDelete(DeleteBehavior.ClientSetNull);
    }
}