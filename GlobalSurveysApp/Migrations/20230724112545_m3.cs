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
            migrationBuilder.DropForeignKey(
                name: "FK_Advance_Users_UserId",
                table: "Advance");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeOff_Users_UserId",
                table: "TimeOff");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TimeOff",
                table: "TimeOff");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Advance",
                table: "Advance");

            migrationBuilder.RenameTable(
                name: "TimeOff",
                newName: "TimeOffs");

            migrationBuilder.RenameTable(
                name: "Advance",
                newName: "Advances");

            migrationBuilder.RenameIndex(
                name: "IX_TimeOff_UserId",
                table: "TimeOffs",
                newName: "IX_TimeOffs_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Advance_UserId",
                table: "Advances",
                newName: "IX_Advances_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TimeOffs",
                table: "TimeOffs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Advances",
                table: "Advances",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "PublicList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAR = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActived = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Advances_Users_UserId",
                table: "Advances",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeOffs_Users_UserId",
                table: "TimeOffs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advances_Users_UserId",
                table: "Advances");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeOffs_Users_UserId",
                table: "TimeOffs");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "PublicList");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Users_RoleId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TimeOffs",
                table: "TimeOffs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Advances",
                table: "Advances");

            migrationBuilder.RenameTable(
                name: "TimeOffs",
                newName: "TimeOff");

            migrationBuilder.RenameTable(
                name: "Advances",
                newName: "Advance");

            migrationBuilder.RenameIndex(
                name: "IX_TimeOffs_UserId",
                table: "TimeOff",
                newName: "IX_TimeOff_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Advances_UserId",
                table: "Advance",
                newName: "IX_Advance_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TimeOff",
                table: "TimeOff",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Advance",
                table: "Advance",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Advance_Users_UserId",
                table: "Advance",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeOff_Users_UserId",
                table: "TimeOff",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
