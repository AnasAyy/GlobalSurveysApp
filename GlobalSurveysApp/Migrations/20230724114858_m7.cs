using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GlobalSurveysApp.Migrations
{
    /// <inheritdoc />
    public partial class m7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeOffs_Users_UserId1",
                table: "TimeOffs");

            migrationBuilder.DropIndex(
                name: "IX_TimeOffs_UserId1",
                table: "TimeOffs");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "TimeOffs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "TimeOffs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TimeOffs_UserId1",
                table: "TimeOffs",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeOffs_Users_UserId1",
                table: "TimeOffs",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
