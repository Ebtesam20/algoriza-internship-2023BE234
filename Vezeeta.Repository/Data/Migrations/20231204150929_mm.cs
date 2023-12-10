using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vezeeta.Repository.Data.Migrations
{
    public partial class mm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {




            migrationBuilder.DropTable(
               name: "Bookings");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
          
     
        }
    }
}
