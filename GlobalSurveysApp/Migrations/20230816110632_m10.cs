using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GlobalSurveysApp.Migrations
{
    /// <inheritdoc />
    public partial class m10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepartmentName",
                table: "Complaints");

            migrationBuilder.AddColumn<string>(
                name: "Against",
                table: "Complaints",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Complaints",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Against",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Complaints");

            migrationBuilder.AddColumn<int>(
                name: "DepartmentName",
                table: "Complaints",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
