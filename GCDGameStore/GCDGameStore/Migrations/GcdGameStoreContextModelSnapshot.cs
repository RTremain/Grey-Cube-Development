﻿// <auto-generated />
using System;
using GCDGameStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GCDGameStore.Migrations
{
    [DbContext(typeof(GcdGameStoreContext))]
    partial class GcdGameStoreContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.3-rtm-32065")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GCDGameStore.Models.Employee", b =>
                {
                    b.Property<int>("EmployeeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("employeeId")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnName("name")
                        .HasMaxLength(255);

                    b.Property<string>("PwHash")
                        .HasColumnName("pwHash")
                        .HasMaxLength(30);

                    b.HasKey("EmployeeId")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.ToTable("employee");
                });

            modelBuilder.Entity("GCDGameStore.Models.Game", b =>
                {
                    b.Property<int>("GameID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("ReleaseDate");

                    b.Property<string>("Title");

                    b.HasKey("GameID");

                    b.ToTable("Game");
                });
#pragma warning restore 612, 618
        }
    }
}
