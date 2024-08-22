using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infracstructures.FluentAPIs;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("Account");

        //Id
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasDefaultValueSql("NEWID()");
        //Email
        builder.Property(u => u.Email).IsRequired().HasMaxLength(50);
        builder.HasIndex(x => x.Email).IsUnique();

        //Password
        builder.Property(u => u.Password).IsRequired().HasMaxLength(100);

        //Phone
        builder.Property(u => u.Phone).HasMaxLength(10);

        //Birthday 
        builder.Property(u => u.Birthday).IsRequired();

        //Avatar
        builder.Property(u => u.Avatar);

        //FullName 
        builder.Property(u => u.FullName).HasMaxLength(30).HasDefaultValue("");

        //Gender
        builder.Property(u => u.Gender).IsRequired().HasDefaultValue("False");

        //Role
        builder.Property(u => u.Role).IsRequired();

        //Address
        builder.Property(u => u.Address).HasDefaultValue("");

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


        //Relation
        builder.HasMany(u => u.CreateContest).WithOne(u => u.Account).HasForeignKey(u => u.StaffId)
            .OnDelete(DeleteBehavior.ClientSetNull);
        builder.HasMany(u => u.Collection).WithOne(u => u.Account).HasForeignKey(u => u.AccountId)
            .OnDelete(DeleteBehavior.ClientSetNull);
        builder.HasMany(u => u.Collection).WithOne(u => u.Account).HasForeignKey(u => u.AccountId)
            .OnDelete(DeleteBehavior.ClientSetNull);
        builder.HasMany(u => u.Report).WithOne(u => u.Account).HasForeignKey(u => u.CompetitorId)
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}