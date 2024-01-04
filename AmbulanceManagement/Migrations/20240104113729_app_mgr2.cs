using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmbulanceManagement.Migrations
{
    public partial class app_mgr2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppointmentDateTime",
                table: "Appointment");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AppointmentDateTime",
                table: "Appointment",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
