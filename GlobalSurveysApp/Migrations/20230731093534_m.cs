using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GlobalSurveysApp.Migrations
{
    /// <inheritdoc />
    public partial class m : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Parent",
                table: "PublicLists");

            migrationBuilder.AddColumn<string>(
                name: "IdCard",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PersonalPhoto",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "PublicLists",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdCard",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PersonalPhoto",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "PublicLists",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Parent",
                table: "PublicLists",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
