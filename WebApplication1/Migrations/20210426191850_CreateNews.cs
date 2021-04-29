using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace WebApplication1.Migrations
{
    public partial class CreateNews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Img = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Desc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FavImg = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductHref = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateData = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "News");
        }
    }
}
