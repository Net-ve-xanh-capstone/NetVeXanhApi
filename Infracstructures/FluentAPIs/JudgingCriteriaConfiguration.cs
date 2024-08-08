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
    public class JudgingCriteriaConfiguration
    {
        public void Configure(EntityTypeBuilder<JudgingCriteria> builder)
        {
            builder.ToTable("JudgingCriteria");

            //Id
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                .HasDefaultValueSql("NEWID()");

            //Name
            builder.Property(u => u.Name);

        }
    }
}
