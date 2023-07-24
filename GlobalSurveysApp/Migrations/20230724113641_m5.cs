using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GlobalSurveysApp.Migrations
{
    /// <inheritdoc />
    public partial class m5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeOffs_Users_UserId",
                table: "TimeOffs");

            migrationBuilder.RenameColumn(
                name: "SubstituteEmployee",
                table: "TimeOffs",
                newName: "SubstituteEmployeeId");

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "TimeOffs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TimeOffs_SubstituteEmployeeId",
                table: "TimeOffs",
                column: "SubstituteEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeOffs_UserId1",
                table: "TimeOffs",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeOffs_Users_SubstituteEmployeeId",
                table: "TimeOffs",
                column: "SubstituteEmployeeId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeOffs_Users_UserId",
                table: "TimeOffs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeOffs_Users_UserId1",
                table: "TimeOffs",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeOffs_Users_SubstituteEmployeeId",
                table: "TimeOffs");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeOffs_Users_UserId",
                table: "TimeOffs");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeOffs_Users_UserId1",
                table: "TimeOffs");

            migrationBuilder.DropIndex(
                name: "IX_TimeOffs_SubstituteEmployeeId",
                table: "TimeOffs");

            migrationBuilder.DropIndex(
                name: "IX_TimeOffs_UserId1",
                table: "TimeOffs");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "TimeOffs");

            migrationBuilder.RenameColumn(
                name: "SubstituteEmployeeId",
                table: "TimeOffs",
                newName: "SubstituteEmployee");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeOffs_Users_UserId",
                table: "TimeOffs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
