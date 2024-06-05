﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infracstructures.FluentAPIs
{
    public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
    {
        public void Configure(EntityTypeBuilder<Schedule> builder)
        {
            builder.ToTable("Schedule");

            //Id
            builder.HasKey(u => u.Id);

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

            //Description
            builder.Property(u => u.Description);

            //RoundId
            builder.Property(u => u.RoundId).IsRequired();

            //ExaminerId
            builder.Property(u => u.ExaminerId).IsRequired();




            //Relation
            builder.HasOne(u => u.Account).WithMany(u => u.Schedule).HasForeignKey(u => u.ExaminerId);
        }
    }
}
