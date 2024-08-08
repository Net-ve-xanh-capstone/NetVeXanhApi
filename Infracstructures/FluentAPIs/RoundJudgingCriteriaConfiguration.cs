using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infracstructures.FluentAPIs
{
    public class RoundJudgingCriteriaConfiguration
    {
        public void Configure(EntityTypeBuilder<RoundJudgingCriteria> builder)
        {
            builder.ToTable("RoundJudgingCriteria");

            //Id
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                .HasDefaultValueSql("NEWID()");

            //Description
            builder.Property(u => u.Description);

            //RoundId
            builder.Property(u => u.RoundId).IsRequired();
            //JudgingCriteriaId
            builder.Property(u => u.JudgingCriteriaId).IsRequired();

            // Relation
            builder.HasOne(u => u.Round).WithMany(u => u.RoundJudgingCriteria).HasForeignKey(u => u.RoundId)
            .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(u => u.JudgingCriteria).WithMany(u => u.RoundJudgingCriteria).HasForeignKey(u => u.JudgingCriteriaId)
            .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
