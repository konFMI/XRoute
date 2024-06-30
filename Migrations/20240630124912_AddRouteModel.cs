using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XRoute.Migrations
{
    /// <inheritdoc />
    public partial class AddRouteModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Distance",
                table: "Routes");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Routes",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateAdded",
                table: "Routes",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Routes",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RepresentativeUsername",
                table: "Routes",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "DateAdded",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "RepresentativeUsername",
                table: "Routes");

            migrationBuilder.AddColumn<double>(
                name: "Distance",
                table: "Routes",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
