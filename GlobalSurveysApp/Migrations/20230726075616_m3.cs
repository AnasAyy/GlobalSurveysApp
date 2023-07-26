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
            migrationBuilder.DropPrimaryKey(
                name: "PK_PublicList",
                table: "PublicList");

            migrationBuilder.RenameTable(
                name: "PublicList",
                newName: "PublicLists");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PublicLists",
                table: "PublicLists",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PublicLists",
                table: "PublicLists");

            migrationBuilder.RenameTable(
                name: "PublicLists",
                newName: "PublicList");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PublicList",
                table: "PublicList",
                column: "Id");
        }
    }
}
