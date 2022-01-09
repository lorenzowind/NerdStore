using Microsoft.EntityFrameworkCore.Migrations;

namespace NS.Cart.API.Migrations
{
    public partial class Voucher : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AppliedVoucher",
                table: "CustomerCarts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "Discount",
                table: "CustomerCarts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "DiscountType",
                table: "CustomerCarts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountValue",
                table: "CustomerCarts",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Percentage",
                table: "CustomerCarts",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VoucherCode",
                table: "CustomerCarts",
                type: "varchar(50)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppliedVoucher",
                table: "CustomerCarts");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "CustomerCarts");

            migrationBuilder.DropColumn(
                name: "DiscountType",
                table: "CustomerCarts");

            migrationBuilder.DropColumn(
                name: "DiscountValue",
                table: "CustomerCarts");

            migrationBuilder.DropColumn(
                name: "Percentage",
                table: "CustomerCarts");

            migrationBuilder.DropColumn(
                name: "VoucherCode",
                table: "CustomerCarts");
        }
    }
}
