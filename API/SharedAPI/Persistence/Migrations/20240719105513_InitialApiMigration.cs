using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.SharedAPI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialApiMigration : Migration
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
                name: "grocerylistitems",
                columns: table => new
                {
                    itemid = table.Column<string>(type: "text", nullable: false),
                    grocerylistid = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    amount = table.Column<string>(type: "text", nullable: false),
                    picture = table.Column<string>(type: "text", nullable: false),
                    grocerylistmodelgrocerylistid = table.Column<string>(type: "text", nullable: true),
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
                    table.ForeignKey(
                        name: "fk_grocerylistitems_grocerylists_grocerylistmodelgrocerylistid",
                        column: x => x.grocerylistmodelgrocerylistid,
                        principalTable: "grocerylists",
                        principalColumn: "grocerylistid");
                });

            migrationBuilder.CreateIndex(
                name: "ix_grocerylistitems_grocerylistid",
                table: "grocerylistitems",
                column: "grocerylistid");

            migrationBuilder.CreateIndex(
                name: "ix_grocerylistitems_grocerylistmodelgrocerylistid",
                table: "grocerylistitems",
                column: "grocerylistmodelgrocerylistid");

            migrationBuilder.CreateIndex(
                name: "ix_grocerylists_househouldid",
                table: "grocerylists",
                column: "househouldid");

            migrationBuilder.CreateIndex(
                name: "ix_invitations_householdid",
                table: "invitations",
                column: "householdid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "grocerylistitems");

            migrationBuilder.DropTable(
                name: "HouseholdUsers");

            migrationBuilder.DropTable(
                name: "invitations");

            migrationBuilder.DropTable(
                name: "grocerylists");

            migrationBuilder.DropTable(
                name: "households");
        }
    }
}
