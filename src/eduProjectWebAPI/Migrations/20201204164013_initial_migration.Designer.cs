﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using eduProjectWebAPI.Data;

namespace eduProjectWebAPI.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20201204164013_initial_migration")]
    partial class initial_migration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("eduProjectModel.Domain.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");

                    b.HasData(
                        new
                        {
                            Id = "9",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "255fd95b-c771-4be6-852c-5634244b17ac",
                            Email = "nikolinagrahovac@test.com",
                            EmailConfirmed = false,
                            LockoutEnabled = true,
                            NormalizedEmail = "NIKOLINAGRAHOVAC@TEST.COM",
                            NormalizedUserName = "NIKOLINAGRAHOVAC@TEST.COM",
                            PasswordHash = "AQAAAAEAACcQAAAAEB3fErhGcr/yNJzrnpwkMN0eSAjxvNKxRhnu+pc/nKkNmCuBIFm9Hb6ow4nuD2EBrA==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "BRPZ5G5VMPU533RX5YC3S62EYN5H22EL",
                            TwoFactorEnabled = false,
                            UserName = "nikolinagrahovac@test.com"
                        },
                        new
                        {
                            Id = "10",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "80088041-9c29-474a-8668-3277462c4d51",
                            Email = "zoranpantos@test.com",
                            EmailConfirmed = false,
                            LockoutEnabled = true,
                            NormalizedEmail = "ZORANPANTOS@TEST.COM",
                            NormalizedUserName = "ZORANPANTOS@TEST.COM",
                            PasswordHash = "AQAAAAEAACcQAAAAEFhPDeoQp9iRyCWYDdk+1+BVvrNObJl/WW/vJBiezvzhe69X92GG4+z9XUzHS8qN3A==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "7ZTQGH5Q7AX4CPI73DWSQS2OMESYR6KT",
                            TwoFactorEnabled = false,
                            UserName = "zoranpantos@test.com"
                        },
                        new
                        {
                            Id = "11",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "1dd40535-18b6-47ec-bb00-d0b770505ad1",
                            Email = "nikolinamaksimovic@test.com",
                            EmailConfirmed = false,
                            LockoutEnabled = true,
                            NormalizedEmail = "NIKOLINAMAKSIMOVIC@TEST.COM",
                            NormalizedUserName = "NIKOLINAMAKSIMOVIC@TEST.COM",
                            PasswordHash = "AQAAAAEAACcQAAAAEKNjZLA00+meOz9bw9rokp6svoTGUBUu2psELguBARlsD8aQ2DULpNZwMgnvfv4amw==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "ZW5I3YK2S6L6WZCX3WNE24LITMDVRXA5",
                            TwoFactorEnabled = false,
                            UserName = "nikolinamaksimovic@test.com"
                        },
                        new
                        {
                            Id = "12",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "f8e8dfc8-f20e-44fe-9d46-2e322c1d41df",
                            Email = "branislavkljajic@test.com",
                            EmailConfirmed = false,
                            LockoutEnabled = true,
                            NormalizedEmail = "BRANISLAVKLJAJIC@TEST.COM",
                            NormalizedUserName = "BRANISLAVKLJAJIC@TEST.COM",
                            PasswordHash = "AQAAAAEAACcQAAAAEKNDYYoI7EzH7+XXPzTmscNqBXX0sM8WqCejUG2K9Uj0vGPHrMzlghVlhiA1Zx5iLA==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "7XZHWB2VVO2IAE6OLBPGLASCGPBD7GVD",
                            TwoFactorEnabled = false,
                            UserName = "branislavkljajic@test.com"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("eduProjectModel.Domain.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("eduProjectModel.Domain.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("eduProjectModel.Domain.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("eduProjectModel.Domain.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
