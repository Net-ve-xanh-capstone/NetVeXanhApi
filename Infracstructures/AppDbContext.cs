﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infracstructures
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }


        public DbSet<Account> Account { get; set; }
        public DbSet<Award> Award { get; set; }
        public DbSet<Collection> Collection { get; set; }
        public DbSet<Contest> Contest { get; set; }
        public DbSet<EducationalLevel> EducationalLevel { get; set; }
        public DbSet<Competitor> Guardian { get; set; }
        public DbSet<Image> Image { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<Painting> Painting { get; set; }
        public DbSet<PaintingCollection> PaintingCollection { get; set; }
        public DbSet<Post> Post { get; set; }
        public DbSet<PostImage> PostImage { get; set; }
        public DbSet<Resources> Resources { get; set; }
        public DbSet<Round> Round { get; set; }
        public DbSet<Schedule> Schedule { get; set; }
        public DbSet<Sponsor> Sponsor { get; set; }
        public DbSet<Topic> Topic { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();
                var connectionString = configuration.GetConnectionString("NetVeXanh");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
    }
}
