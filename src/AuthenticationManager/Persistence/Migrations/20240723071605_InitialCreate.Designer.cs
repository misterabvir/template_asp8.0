﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Persistence.Contexts;

#nullable disable

namespace Persistence.Migrations
{
    [DbContext(typeof(AuthenticationDbContext))]
    [Migration("20240723071605_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("authentication_schema")
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Domain.UserAggregate.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("user_id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("created_at");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasColumnName("role");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasColumnName("status");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.ToTable("users", "authentication_schema");
                });

            modelBuilder.Entity("Domain.UserAggregate.User", b =>
                {
                    b.OwnsOne("Domain.UserAggregate.Entities.Data", "Data", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("uniqueidentifier")
                                .HasColumnName("user_id");

                            b1.Property<string>("Email")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)")
                                .HasColumnName("email");

                            b1.Property<byte[]>("Password")
                                .IsRequired()
                                .HasColumnType("varbinary(max)")
                                .HasColumnName("password");

                            b1.Property<byte[]>("Salt")
                                .IsRequired()
                                .HasColumnType("varbinary(max)")
                                .HasColumnName("salt");

                            b1.Property<string>("Username")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)")
                                .HasColumnName("username");

                            b1.HasKey("Id")
                                .HasName("pk_users_data");

                            b1.ToTable("user_data", "authentication_schema");

                            b1.WithOwner()
                                .HasForeignKey("Id")
                                .HasConstraintName("fk_user_data");
                        });

                    b.OwnsOne("Domain.UserAggregate.Entities.Profile", "Profile", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("uniqueidentifier")
                                .HasColumnName("user_id");

                            b1.Property<string>("Bio")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("bio");

                            b1.Property<DateOnly>("Birthday")
                                .HasColumnType("date")
                                .HasColumnName("birthday");

                            b1.Property<string>("CoverPicture")
                                .IsRequired()
                                .HasMaxLength(255)
                                .HasColumnType("nvarchar(255)")
                                .HasColumnName("cover_picture");

                            b1.Property<string>("FirstName")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)")
                                .HasColumnName("first_name");

                            b1.Property<string>("Gender")
                                .IsRequired()
                                .HasMaxLength(16)
                                .HasColumnType("nvarchar(16)")
                                .HasColumnName("gender");

                            b1.Property<string>("LastName")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)")
                                .HasColumnName("last_name");

                            b1.Property<string>("Location")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)")
                                .HasColumnName("location");

                            b1.Property<string>("ProfilePicture")
                                .IsRequired()
                                .HasMaxLength(255)
                                .HasColumnType("nvarchar(255)")
                                .HasColumnName("profile_picture");

                            b1.Property<string>("Website")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)")
                                .HasColumnName("website");

                            b1.HasKey("Id")
                                .HasName("pk_profiles");

                            b1.ToTable("user_profiles", "authentication_schema");

                            b1.WithOwner()
                                .HasForeignKey("Id")
                                .HasConstraintName("fk_user_profile");
                        });

                    b.Navigation("Data")
                        .IsRequired();

                    b.Navigation("Profile")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}