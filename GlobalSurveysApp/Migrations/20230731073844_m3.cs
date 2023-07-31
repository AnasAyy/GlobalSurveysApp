using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GlobalSurveysApp.Migrations
{
    /// <inheritdoc />
    public partial class m3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Users",
                newName: "WorkMobile");

            migrationBuilder.RenameColumn(
                name: "NameEn",
                table: "PublicLists",
                newName: "NameEN");

            migrationBuilder.AddColumn<int>(
                name: "CertificateLevel",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "FieldOfStudy",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstContractDate",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Nationality",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PassportNumber",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Postion",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PrivateMobile",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "placeOfBirth",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Parent",
                table: "PublicLists",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CertificateLevel",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FieldOfStudy",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FirstContractDate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Nationality",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PassportNumber",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Postion",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PrivateMobile",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "placeOfBirth",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Parent",
                table: "PublicLists");

            migrationBuilder.RenameColumn(
                name: "WorkMobile",
                table: "Users",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "NameEN",
                table: "PublicLists",
                newName: "NameEn");
        }
    }
}
