using System;
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
        public DbSet<Review> Review { get; set; }
        public DbSet<Rating> Rating { get; set; }


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

            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Event)
                .WithMany(e => e.Attendees)
                .HasForeignKey(a => a.EventId)
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

            
        }
    }
}
