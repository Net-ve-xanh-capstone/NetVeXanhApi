﻿using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infracstructures.FluentAPIs;

public class PaintingConfiguration : IEntityTypeConfiguration<Painting>
{
    public void Configure(EntityTypeBuilder<Painting> builder)
    {
        builder.ToTable("Painting");

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

        //Image
        builder.Property(u => u.Image);

        //Name
        builder.Property(u => u.Name);

        //Description
        builder.Property(u => u.Description);

        //Submitted Timestamp
        builder.Property(u => u.SubmittedTimestamp);

        //FinalDecision Timestamp
        builder.Property(u => u.FinalDecisionTimestamp);
        
        //Reviewed Timestamp
        builder.Property(u => u.ReviewedTimestamp);
        
        //AwardId
        builder.Property(u => u.AwardId);

        //RoundId
        builder.Property(u => u.RoundId).IsRequired();

        //AccountId
        builder.Property(u => u.AccountId).IsRequired();

        //TopicId
        builder.Property(u => u.TopicId).IsRequired();

        //ScheduleId
        builder.Property(u => u.ScheduleId);

        //Code
        builder.Property(u => u.Code);

        //Relation

        builder.HasOne(u => u.Account)
            .WithMany(u => u.Painting)
            .HasForeignKey(u => u.AccountId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        builder.HasOne(u => u.Award)
            .WithMany(u => u.Painting)
            .HasForeignKey(u => u.AwardId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        builder.HasOne(u => u.Schedule)
            .WithMany(u => u.Painting)
            .HasForeignKey(u => u.ScheduleId)
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}