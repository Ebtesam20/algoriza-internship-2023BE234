using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vezeeta.Repository.Data.Migrations
{
    public partial class AddSpecializeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          
            migrationBuilder.CreateTable(
                name: "Specializations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpecializeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specializations", x => x.Id);
                });
            migrationBuilder.InsertData(
                table: "Specializations",
                columns: new[] { "SpecializeName" },
                values: new object[] { "Dermatology" }
            );
            migrationBuilder.InsertData(
               table: "Specializations",
               columns: new[] { "SpecializeName" },
               values: new object[] { "Dentistry" }
           );
            migrationBuilder.InsertData(
              table: "Specializations",
              columns: new[] { "SpecializeName" },
              values: new object[] { "Neurology" }
          );
            migrationBuilder.InsertData(
              table: "Specializations",
              columns: new[] { "SpecializeName" },
              values: new object[] { "Ear,Nose and Throat" }
          );
            migrationBuilder.InsertData(
              table: "Specializations",
              columns: new[] { "SpecializeName" },
              values: new object[] { "Heart" }
          );
            migrationBuilder.InsertData(
              table: "Specializations",
              columns: new[] { "SpecializeName" },
              values: new object[] { "Orthopedics" }
          );
            migrationBuilder.InsertData(
              table: "Specializations",
              columns: new[] { "SpecializeName" },
              values: new object[] { "General Surgery" }
          );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Specializations");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "AspNetUsers",
                type: "decimal(18,2)",
                nullable: true);
        }
    }
}
