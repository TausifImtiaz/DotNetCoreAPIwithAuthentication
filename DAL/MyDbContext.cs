using DotNetCoreAPIwithAuthentication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace DotNetCoreAPIwithAuthentication.DAL
{
    public class MyDbContext : IdentityDbContext<IdentityUser>
    {
        public MyDbContext()
        {
        }

        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        public DbSet<Plant> Plants { get; set; }
        public DbSet<OrderMaster> OrderMasters { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<IdentityUserLogin<string>>();
            modelBuilder.Ignore<IdentityUserRole<string>>();
            modelBuilder.Ignore<IdentityUserToken<string>>();
            modelBuilder.Entity<OrderDetail>()
                .HasOne(d => d.OrderMaster)
                .WithMany(o => o.OrderDetail)
                .HasForeignKey(o => o.OrderId);

            // Plants
            modelBuilder.Entity<Plant>().HasData(
                new Plant { PlantId = 1, Name = "Mango" },
                new Plant { PlantId = 2, Name = "Jasmine" },
                new Plant { PlantId = 3, Name = "Aeromatic Jui" }
            );

            // OrderMasters
            modelBuilder.Entity<OrderMaster>().HasData(
                new OrderMaster
                {
                    OrderId = 1,
                    CustomerName = "Moin Khan",
                    OrderDate = DateTime.Now,
                    IsComplete = true
                },
                new OrderMaster
                {
                    OrderId = 2,
                    CustomerName = "Shorob Ali",
                    OrderDate = DateTime.Now.AddDays(-1),
                    IsComplete = false
                }
            );

            // OrderDetails
            modelBuilder.Entity<OrderDetail>().HasData(
                new OrderDetail
                {
                    DetailId = 1,
                    OrderId = 1,
                    PlantId = 1,
                    Quantity = 1,
                    Price = 255
                },
                new OrderDetail
                {
                    DetailId = 2,
                    OrderId = 1,
                    PlantId = 2,
                    Quantity = 2,
                    Price = 165
                },
                new OrderDetail
                {
                    DetailId = 3,
                    OrderId = 2,
                    PlantId = 3,
                    Quantity = 3,
                    Price = 400
                }
            );
        }
    }
}