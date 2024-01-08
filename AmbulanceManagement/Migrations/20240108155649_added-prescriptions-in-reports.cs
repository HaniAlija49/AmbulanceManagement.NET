using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmbulanceManagement.Migrations
{
    public partial class addedprescriptionsinreports : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Prescriptions",
                table: "Report",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Prescriptions",
                table: "Report");
        }
    }
}
