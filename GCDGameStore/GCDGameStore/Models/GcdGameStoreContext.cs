using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

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

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().ToTable("Employee");

            modelBuilder.Entity<Game>().ToTable("Game");
        }
    }
}
