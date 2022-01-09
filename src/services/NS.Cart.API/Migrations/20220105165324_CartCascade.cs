using Microsoft.EntityFrameworkCore.Migrations;

namespace NS.Cart.API.Migrations
{
    public partial class CartCascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItens_CustomerCarts_CartId",
                table: "CartItens");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItens_CustomerCarts_CartId",
                table: "CartItens",
                column: "CartId",
                principalTable: "CustomerCarts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItens_CustomerCarts_CartId",
                table: "CartItens");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItens_CustomerCarts_CartId",
                table: "CartItens",
                column: "CartId",
                principalTable: "CustomerCarts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
