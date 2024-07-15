using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.SharedAPI.Persistence.Migrations
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
                name: "householdusers",
                columns: table => new
                {
                    householdid = table.Column<string>(type: "text", nullable: false),
                    userid = table.Column<string>(type: "text", nullable: false),
                    role = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_householdusers", x => new { x.householdid, x.userid });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "households");

            migrationBuilder.DropTable(
                name: "householdusers");
        }
    }
}
