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
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.EmployeeId)
                    .ForSqlServerIsClustered(false);

                entity.ToTable("employee");

                entity.Property(e => e.EmployeeId).HasColumnName("employeeId");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255);

                entity.Property(e => e.PwHash)
                    .HasColumnName("pwHash")
                    .HasMaxLength(30);
            });

            modelBuilder.Entity<Game>().ToTable("Game");
        }
    }
}
