using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infracstructures.FluentAPIs;

internal class RoundConfiguration : IEntityTypeConfiguration<Round>
{
    public void Configure(EntityTypeBuilder<Round> builder)
    {
        builder.ToTable("Round");

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

        //Name 
        builder.Property(u => u.Name);

        //StartTime
        builder.Property(u => u.StartTime);

        //EndTime
        builder.Property(u => u.EndTime);

        //Location
        builder.Property(u => u.Location).HasDefaultValue("Không có thông tin");

        //Description
        builder.Property(u => u.Description).HasDefaultValue("Không có mô tả");

        //EducationalLevel
        builder.Property(u => u.EducationalLevelId);

        //RoundNumber
        builder.Property(u => u.RoundNumber);

        //DeadlineSubmissionDate
        builder.Property(u => u.DeadlineSubmissionDate);

        //ResultAnnouncementDate
        builder.Property(u => u.ResultAnnouncementDate);




        //Relation
        builder.HasMany(u => u.Schedule).WithOne(u => u.Round).HasForeignKey(u => u.RoundId)
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}