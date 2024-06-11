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
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("Post");

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

            //Url
            builder.Property(u => u.Url);

            //Title
            builder.Property(u => u.Title);

            //Description
            builder.Property(u => u.Description);

            //StaffId
            builder.Property(u => u.StaffId).IsRequired();



            builder.HasMany(u => u.PostImages).WithOne(u => u.Post).HasForeignKey(u => u.PostId).OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(u => u.Account).WithMany(u => u.Post).HasForeignKey(u => u.StaffId).OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
