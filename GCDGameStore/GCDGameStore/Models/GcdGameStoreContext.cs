﻿using System;
using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Metadata;
using GCDGameStore.Models;
using GCDGameStore.ViewModels;

namespace GCDGameStore.Models
{
    public partial class GcdGameStoreContext : DbContext
    {
        public GcdGameStoreContext()
        {
        }

        public GcdGameStoreContext(DbContextOptions<GcdGameStoreContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<Game> Game { get; set; }
        public virtual DbSet<Event> Event { get; set; }
        public virtual DbSet<Member> Member { get; set; }
        public virtual DbSet<Library> Library { get; set; }
        public virtual DbSet<Wishlist> Wishlist { get; set; }
        public virtual DbSet<CreditCard> CreditCard { get; set; }
        public virtual DbSet<Friend> Friend { get; set; }
        public virtual DbSet<Attendance> Attendance { get; set; }
        public virtual DbSet<Review> Review { get; set; }
        public virtual DbSet<Rating> Rating { get; set; }
        public virtual DbSet<ResetPasswordVerify> ResetPasswordVerify {get; set;}
        public virtual DbSet<Shipment> Shipment { get; set; }
        public virtual DbSet<ShipItem> ShipItem { get; set; }
        public virtual DbSet<Platform> Platform { get; set; }
        public virtual DbSet<Genre> Genre { get; set; }
        public virtual DbSet<MemberPlatform> MemberPlatform { get; set; }
        public virtual DbSet<MemberGenre> MemberGenre { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<OrderItemDigital> OrderItemDigital { get; set; }
        public virtual DbSet<OrderItemPhysical> OrderItemPhysical { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().ToTable("Employee");

            modelBuilder.Entity<Game>().ToTable("Game");
            modelBuilder.Entity<Event>().ToTable("Event");
            modelBuilder.Entity<Member>().ToTable("Member");
            modelBuilder.Entity<Library>().ToTable("Library");
            modelBuilder.Entity<Wishlist>().ToTable("Wishlist");
            modelBuilder.Entity<CreditCard>().ToTable("CreditCard");
            modelBuilder.Entity<Review>().ToTable("Review");
            modelBuilder.Entity<Rating>().ToTable("Rating");
            modelBuilder.Entity<ResetPasswordVerify>().ToTable("ResetPasswordVerify");

            modelBuilder.Entity<Platform>().ToTable("Platform");
            modelBuilder.Entity<Genre>().ToTable("Genre");
            modelBuilder.Entity<MemberPlatform>().ToTable("MemberPlatform");
            modelBuilder.Entity<MemberGenre>().ToTable("MemberGenre");
            modelBuilder.Entity<Order>().ToTable("Order");

            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Event)
                .WithMany(e => e.Attendees)
                .HasForeignKey(a => a.EventId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ShipItem>()
                .HasOne(si => si.Shipment)
                .WithMany(s => s.ShipItems)
                .HasForeignKey(si => si.ShipmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Shipment>()
                .HasOne(s => s.Employee)
                .WithMany(e => e.Shipments)
                .HasForeignKey(s => s.EmployeeId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
            

            modelBuilder.Entity<Friend>()
                .HasOne(f => f.Member)
                .WithMany(m => m.MyFriends)
                .HasForeignKey(f => f.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Friend>()
                .HasOne(f => f.FriendMember)
                .WithMany(m => m.FriendsOf)
                .HasForeignKey(f => f.FriendMemberId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderItemPhysical>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.PhysicalItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderItemDigital>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.DigitalItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Restrict);


        }


        public DbSet<GCDGameStore.ViewModels.MemberPlatformViewModel> MemberPlatformViewModel { get; set; }


        public DbSet<GCDGameStore.ViewModels.MemberGenreViewModel> MemberGenreViewModel { get; set; }
    }
}
