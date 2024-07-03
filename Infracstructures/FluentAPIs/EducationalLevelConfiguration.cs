﻿using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infracstructures.FluentAPIs;

public class EducationalLevelConfiguration : IEntityTypeConfiguration<EducationalLevel>
{
    public void Configure(EntityTypeBuilder<EducationalLevel> builder)
    {
        builder.ToTable("EducationalLevel");

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
        builder.Property(u => u.Status).HasDefaultValue("False");

        //ContestId
        builder.Property(u => u.ContestId).IsRequired();

        //StartTime
        builder.Property(u => u.StartTime);

        //EndTime
        builder.Property(u => u.EndTime);

        //Description
        builder.Property(u => u.Description);

        //Level
        builder.Property(u => u.Level);


        //Relation
        builder.HasMany(u => u.Round).WithOne(u => u.EducationalLevel).HasForeignKey(u => u.EducationalLevelId)
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}