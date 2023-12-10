using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vezeeta.Repository.Data.Migrations
{
    public partial class UpdateDiscountId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
               name: "DiscountCodeCouponId",
               table: "Bookings",
               type: "int",
               nullable: true,
               oldClrType: typeof(int),
               oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
