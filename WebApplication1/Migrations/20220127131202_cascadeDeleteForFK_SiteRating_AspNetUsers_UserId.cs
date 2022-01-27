using Microsoft.EntityFrameworkCore.Migrations;

namespace InternetShop.Migrations
{
    public partial class cascadeDeleteForFK_SiteRating_AspNetUsers_UserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SiteRating_AspNetUsers_UserId",
                table: "SiteRating");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "SiteRating",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SiteRating_AspNetUsers_UserId",
                table: "SiteRating",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SiteRating_AspNetUsers_UserId",
                table: "SiteRating");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "SiteRating",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_SiteRating_AspNetUsers_UserId",
                table: "SiteRating",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
