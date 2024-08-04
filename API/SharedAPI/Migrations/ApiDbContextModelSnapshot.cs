﻿// <auto-generated />
using System;
using API.SharedAPI.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace API.SharedAPI.Migrations
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

            modelBuilder.Entity("API.GroceryList.Models.GroceryItemModel", b =>
                {
                    b.Property<string>("ItemId")
                        .HasColumnType("text")
                        .HasColumnName("itemid");

                    b.Property<string>("Amount")
                        .HasColumnType("text")
                        .HasColumnName("amount");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp")
                        .HasColumnName("createdat");

                    b.Property<string>("GroceryListId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("grocerylistid");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("timestamp")
                        .HasColumnName("lastmodified");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("Picture")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("picture");

                    b.HasKey("ItemId")
                        .HasName("pk_grocerylistitems");

                    b.HasIndex("GroceryListId")
                        .HasDatabaseName("ix_grocerylistitems_grocerylistid");

                    b.ToTable("grocerylistitems", (string)null);
                });

            modelBuilder.Entity("API.GroceryList.Models.GroceryListModel", b =>
                {
                    b.Property<string>("GroceryListId")
                        .HasColumnType("text")
                        .HasColumnName("grocerylistid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp")
                        .HasColumnName("createdat");

                    b.Property<string>("HousehouldId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("househouldid");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("timestamp")
                        .HasColumnName("lastmodified");

                    b.HasKey("GroceryListId")
                        .HasName("pk_grocerylists");

                    b.HasIndex("HousehouldId")
                        .HasDatabaseName("ix_grocerylists_househouldid");

                    b.ToTable("grocerylists", (string)null);
                });

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

            modelBuilder.Entity("API.MealPlanner.Models.IngredientModel", b =>
                {
                    b.Property<string>("IngredientId")
                        .HasColumnType("text")
                        .HasColumnName("ingredientid");

                    b.Property<string>("Amount")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("amount");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp")
                        .HasColumnName("createdat");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("timestamp")
                        .HasColumnName("lastmodified");

                    b.Property<string>("MealId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("mealid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("name");

                    b.HasKey("IngredientId")
                        .HasName("pk_ingredients");

                    b.HasIndex("MealId")
                        .HasDatabaseName("ix_ingredients_mealid");

                    b.ToTable("ingredients", (string)null);
                });

            modelBuilder.Entity("API.MealPlanner.Models.MealModel", b =>
                {
                    b.Property<string>("MealId")
                        .HasColumnType("text")
                        .HasColumnName("mealid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp")
                        .HasColumnName("createdat");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("HouseholdId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("householdid");

                    b.Property<string>("Image")
                        .HasColumnType("text")
                        .HasColumnName("image");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("timestamp")
                        .HasColumnName("lastmodified");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("name");

                    b.HasKey("MealId")
                        .HasName("pk_meals");

                    b.HasIndex("HouseholdId")
                        .HasDatabaseName("ix_meals_householdid");

                    b.ToTable("meals", (string)null);
                });

            modelBuilder.Entity("API.MealPlanner.Models.MealPlanModel", b =>
                {
                    b.Property<string>("MealPlanId")
                        .HasColumnType("text")
                        .HasColumnName("mealplanid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp")
                        .HasColumnName("createdat");

                    b.Property<string>("HouseholdId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("householdid");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("timestamp")
                        .HasColumnName("lastmodified");

                    b.Property<DateTime>("WeekNo")
                        .HasColumnType("date")
                        .HasColumnName("weekno");

                    b.HasKey("MealPlanId")
                        .HasName("pk_mealplans");

                    b.HasIndex("HouseholdId")
                        .HasDatabaseName("ix_mealplans_householdid");

                    b.ToTable("mealplans", (string)null);
                });

            modelBuilder.Entity("API.MealPlanner.Models.MealsInPlan", b =>
                {
                    b.Property<string>("MealsInPlanId")
                        .HasColumnType("text")
                        .HasColumnName("mealsinplanid");

                    b.Property<string>("MealId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("mealid");

                    b.Property<string>("MealPlanId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("mealplanid");

                    b.HasKey("MealsInPlanId")
                        .HasName("pk_mealsinplans");

                    b.HasIndex("MealId")
                        .HasDatabaseName("ix_mealsinplans_mealid");

                    b.HasIndex("MealPlanId")
                        .HasDatabaseName("ix_mealsinplans_mealplanid");

                    b.ToTable("mealsinplans", (string)null);
                });

            modelBuilder.Entity("API.GroceryList.Models.GroceryItemModel", b =>
                {
                    b.HasOne("API.GroceryList.Models.GroceryListModel", "GroceryList")
                        .WithMany("Items")
                        .HasForeignKey("GroceryListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_grocerylistitems_grocerylists_grocerylistid");

                    b.Navigation("GroceryList");
                });

            modelBuilder.Entity("API.GroceryList.Models.GroceryListModel", b =>
                {
                    b.HasOne("API.Household.Models.HouseholdModel", "Household")
                        .WithMany()
                        .HasForeignKey("HousehouldId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_grocerylists_households_househouldid");

                    b.Navigation("Household");
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

            modelBuilder.Entity("API.MealPlanner.Models.IngredientModel", b =>
                {
                    b.HasOne("API.MealPlanner.Models.MealModel", "Meal")
                        .WithMany("Ingredients")
                        .HasForeignKey("MealId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_ingredients_meals_mealid");

                    b.Navigation("Meal");
                });

            modelBuilder.Entity("API.MealPlanner.Models.MealModel", b =>
                {
                    b.HasOne("API.Household.Models.HouseholdModel", "Household")
                        .WithMany()
                        .HasForeignKey("HouseholdId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_meals_households_householdid");

                    b.Navigation("Household");
                });

            modelBuilder.Entity("API.MealPlanner.Models.MealPlanModel", b =>
                {
                    b.HasOne("API.Household.Models.HouseholdModel", "Household")
                        .WithMany()
                        .HasForeignKey("HouseholdId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_mealplans_households_householdid");

                    b.Navigation("Household");
                });

            modelBuilder.Entity("API.MealPlanner.Models.MealsInPlan", b =>
                {
                    b.HasOne("API.MealPlanner.Models.MealModel", "Meal")
                        .WithMany()
                        .HasForeignKey("MealId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_mealsinplans_meals_mealid");

                    b.HasOne("API.MealPlanner.Models.MealPlanModel", "MealPlan")
                        .WithMany("Meals")
                        .HasForeignKey("MealPlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_mealsinplans_mealplans_mealplanid");

                    b.Navigation("Meal");

                    b.Navigation("MealPlan");
                });

            modelBuilder.Entity("API.GroceryList.Models.GroceryListModel", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("API.MealPlanner.Models.MealModel", b =>
                {
                    b.Navigation("Ingredients");
                });

            modelBuilder.Entity("API.MealPlanner.Models.MealPlanModel", b =>
                {
                    b.Navigation("Meals");
                });
#pragma warning restore 612, 618
        }
    }
}
