using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.SharedAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialAPIMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "households",
                columns: table => new
                {
                    householdid = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    createdat = table.Column<DateTime>(type: "timestamp", nullable: false),
                    lastmodified = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_households", x => x.householdid);
                });

            migrationBuilder.CreateTable(
                name: "HouseholdUsers",
                columns: table => new
                {
                    userid = table.Column<string>(type: "text", nullable: false),
                    householdid = table.Column<string>(type: "text", nullable: false),
                    role = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_householdusers", x => x.userid);
                });

            migrationBuilder.CreateTable(
                name: "grocerylists",
                columns: table => new
                {
                    grocerylistid = table.Column<string>(type: "text", nullable: false),
                    househouldid = table.Column<string>(type: "text", nullable: false),
                    createdat = table.Column<DateTime>(type: "timestamp", nullable: false),
                    lastmodified = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_grocerylists", x => x.grocerylistid);
                    table.ForeignKey(
                        name: "fk_grocerylists_households_househouldid",
                        column: x => x.househouldid,
                        principalTable: "households",
                        principalColumn: "householdid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "invitations",
                columns: table => new
                {
                    invitationid = table.Column<string>(type: "text", nullable: false),
                    inviteeuserid = table.Column<string>(type: "text", nullable: false),
                    inviteruserid = table.Column<string>(type: "text", nullable: false),
                    householdid = table.Column<string>(type: "text", nullable: false),
                    createdat = table.Column<DateTime>(type: "timestamp", nullable: false),
                    lastmodified = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_invitations", x => x.invitationid);
                    table.ForeignKey(
                        name: "fk_invitations_households_householdid",
                        column: x => x.householdid,
                        principalTable: "households",
                        principalColumn: "householdid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mealplans",
                columns: table => new
                {
                    mealplanid = table.Column<string>(type: "text", nullable: false),
                    householdid = table.Column<string>(type: "text", nullable: false),
                    year = table.Column<int>(type: "integer", nullable: false),
                    week = table.Column<int>(type: "integer", nullable: false),
                    createdat = table.Column<DateTime>(type: "timestamp", nullable: false),
                    lastmodified = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mealplans", x => x.mealplanid);
                    table.UniqueConstraint("ak_mealplans_householdid_week_year", x => new { x.householdid, x.week, x.year });
                    table.ForeignKey(
                        name: "fk_mealplans_households_householdid",
                        column: x => x.householdid,
                        principalTable: "households",
                        principalColumn: "householdid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "meals",
                columns: table => new
                {
                    mealid = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    householdid = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    image = table.Column<string>(type: "text", nullable: true),
                    createdat = table.Column<DateTime>(type: "timestamp", nullable: false),
                    lastmodified = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_meals", x => x.mealid);
                    table.ForeignKey(
                        name: "fk_meals_households_householdid",
                        column: x => x.householdid,
                        principalTable: "households",
                        principalColumn: "householdid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "grocerylistitems",
                columns: table => new
                {
                    itemid = table.Column<string>(type: "text", nullable: false),
                    grocerylistid = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    amount = table.Column<string>(type: "text", nullable: true),
                    picture = table.Column<string>(type: "text", nullable: false),
                    createdat = table.Column<DateTime>(type: "timestamp", nullable: false),
                    lastmodified = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_grocerylistitems", x => x.itemid);
                    table.ForeignKey(
                        name: "fk_grocerylistitems_grocerylists_grocerylistid",
                        column: x => x.grocerylistid,
                        principalTable: "grocerylists",
                        principalColumn: "grocerylistid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ingredients",
                columns: table => new
                {
                    ingredientid = table.Column<string>(type: "text", nullable: false),
                    mealid = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    amount = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    createdat = table.Column<DateTime>(type: "timestamp", nullable: false),
                    lastmodified = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ingredients", x => x.ingredientid);
                    table.ForeignKey(
                        name: "fk_ingredients_meals_mealid",
                        column: x => x.mealid,
                        principalTable: "meals",
                        principalColumn: "mealid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mealsinplans",
                columns: table => new
                {
                    mealsinplanid = table.Column<string>(type: "text", nullable: false),
                    mealid = table.Column<string>(type: "text", nullable: false),
                    mealplanid = table.Column<string>(type: "text", nullable: false),
                    mealday = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mealsinplans", x => x.mealsinplanid);
                    table.ForeignKey(
                        name: "fk_mealsinplans_mealplans_mealplanid",
                        column: x => x.mealplanid,
                        principalTable: "mealplans",
                        principalColumn: "mealplanid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_mealsinplans_meals_mealid",
                        column: x => x.mealid,
                        principalTable: "meals",
                        principalColumn: "mealid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_grocerylistitems_grocerylistid",
                table: "grocerylistitems",
                column: "grocerylistid");

            migrationBuilder.CreateIndex(
                name: "ix_grocerylists_househouldid",
                table: "grocerylists",
                column: "househouldid");

            migrationBuilder.CreateIndex(
                name: "ix_ingredients_mealid",
                table: "ingredients",
                column: "mealid");

            migrationBuilder.CreateIndex(
                name: "ix_invitations_householdid",
                table: "invitations",
                column: "householdid");

            migrationBuilder.CreateIndex(
                name: "ix_meals_householdid",
                table: "meals",
                column: "householdid");

            migrationBuilder.CreateIndex(
                name: "ix_mealsinplans_mealid",
                table: "mealsinplans",
                column: "mealid");

            migrationBuilder.CreateIndex(
                name: "ix_mealsinplans_mealplanid",
                table: "mealsinplans",
                column: "mealplanid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "grocerylistitems");

            migrationBuilder.DropTable(
                name: "HouseholdUsers");

            migrationBuilder.DropTable(
                name: "ingredients");

            migrationBuilder.DropTable(
                name: "invitations");

            migrationBuilder.DropTable(
                name: "mealsinplans");

            migrationBuilder.DropTable(
                name: "grocerylists");

            migrationBuilder.DropTable(
                name: "mealplans");

            migrationBuilder.DropTable(
                name: "meals");

            migrationBuilder.DropTable(
                name: "households");
        }
    }
}
