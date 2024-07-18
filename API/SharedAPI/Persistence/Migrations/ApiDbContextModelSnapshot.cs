﻿// <auto-generated />
using System;
using API.SharedAPI.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace API.SharedAPI.Persistence.Migrations
{
    [DbContext(typeof(ApiDbContext))]
    partial class ApiDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("API.Household.Models.HouseholdModel", b =>
                {
                    b.Property<string>("HouseholdId")
                        .HasColumnType("text")
                        .HasColumnName("householdid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp")
                        .HasColumnName("createdat");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("timestamp")
                        .HasColumnName("lastmodified");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("HouseholdId")
                        .HasName("pk_households");

                    b.ToTable("households", (string)null);
                });

            modelBuilder.Entity("API.Household.Models.HouseholdUsersModel", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text")
                        .HasColumnName("userid");

                    b.Property<string>("HouseholdId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("householdid");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("role");

                    b.HasKey("UserId")
                        .HasName("pk_householdusers");

                    b.ToTable("HouseholdUsers", (string)null);
                });

            modelBuilder.Entity("API.Household.Models.InvitationModel", b =>
                {
                    b.Property<string>("InvitationId")
                        .HasColumnType("text")
                        .HasColumnName("invitationid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp")
                        .HasColumnName("createdat");

                    b.Property<string>("HouseholdId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("householdid");

                    b.Property<string>("InviteeUserId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("inviteeuserid");

                    b.Property<string>("InviterUserId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("inviteruserid");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("timestamp")
                        .HasColumnName("lastmodified");

                    b.HasKey("InvitationId")
                        .HasName("pk_invitations");

                    b.HasIndex("HouseholdId")
                        .HasDatabaseName("ix_invitations_householdid");

                    b.ToTable("invitations", (string)null);
                });

            modelBuilder.Entity("API.Household.Models.InvitationModel", b =>
                {
                    b.HasOne("API.Household.Models.HouseholdModel", "Household")
                        .WithMany()
                        .HasForeignKey("HouseholdId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_invitations_households_householdid");

                    b.Navigation("Household");
                });
#pragma warning restore 612, 618
        }
    }
}
