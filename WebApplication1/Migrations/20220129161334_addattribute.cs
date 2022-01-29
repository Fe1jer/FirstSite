using Microsoft.EntityFrameworkCore.Migrations;

namespace InternetShop.Migrations
{
    public partial class addattribute : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductAttribute_AttributeCategory_AttributeCategoryId",
                table: "ProductAttribute");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductAttribute_Product_ProductId",
                table: "ProductAttribute");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductAttribute",
                table: "ProductAttribute");

            migrationBuilder.RenameTable(
                name: "ProductAttribute",
                newName: "AttributeValue");

            migrationBuilder.RenameColumn(
                name: "AttributeCategoryId",
                table: "AttributeValue",
                newName: "AttributeId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductAttribute_ProductId",
                table: "AttributeValue",
                newName: "IX_AttributeValue_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductAttribute_AttributeCategoryId",
                table: "AttributeValue",
                newName: "IX_AttributeValue_AttributeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AttributeValue",
                table: "AttributeValue",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AttributeValue_AttributeCategory_AttributeId",
                table: "AttributeValue",
                column: "AttributeId",
                principalTable: "AttributeCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AttributeValue_Product_ProductId",
                table: "AttributeValue",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttributeValue_AttributeCategory_AttributeId",
                table: "AttributeValue");

            migrationBuilder.DropForeignKey(
                name: "FK_AttributeValue_Product_ProductId",
                table: "AttributeValue");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AttributeValue",
                table: "AttributeValue");

            migrationBuilder.RenameTable(
                name: "AttributeValue",
                newName: "ProductAttribute");

            migrationBuilder.RenameColumn(
                name: "AttributeId",
                table: "ProductAttribute",
                newName: "AttributeCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_AttributeValue_ProductId",
                table: "ProductAttribute",
                newName: "IX_ProductAttribute_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_AttributeValue_AttributeId",
                table: "ProductAttribute",
                newName: "IX_ProductAttribute_AttributeCategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductAttribute",
                table: "ProductAttribute",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAttribute_AttributeCategory_AttributeCategoryId",
                table: "ProductAttribute",
                column: "AttributeCategoryId",
                principalTable: "AttributeCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAttribute_Product_ProductId",
                table: "ProductAttribute",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
