using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vezeeta.Repository.Data.Migrations
{
    public partial class UpdateBookingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_DiscountCodeCoupons_DiscountCodeCouponId",
                table: "Bookings");

            migrationBuilder.AlterColumn<int>(
                name: "DiscountCodeCouponId",
                table: "Bookings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_DiscountCodeCoupons_DiscountCodeCouponId",
                table: "Bookings",
                column: "DiscountCodeCouponId",
                principalTable: "DiscountCodeCoupons",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_DiscountCodeCoupons_DiscountCodeCouponId",
                table: "Bookings");

            migrationBuilder.AlterColumn<int>(
                name: "DiscountCodeCouponId",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_DiscountCodeCoupons_DiscountCodeCouponId",
                table: "Bookings",
                column: "DiscountCodeCouponId",
                principalTable: "DiscountCodeCoupons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
