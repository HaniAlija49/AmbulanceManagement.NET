using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmbulanceManagement.Migrations
{
    public partial class Appointmentreport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PatientId",
                table: "Report",
                newName: "AppointmentId");

            migrationBuilder.AlterColumn<string>(
                name: "DoctorId",
                table: "Report",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Report_AppointmentId",
                table: "Report",
                column: "AppointmentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Report_DoctorId",
                table: "Report",
                column: "DoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Report_Appointment_AppointmentId",
                table: "Report",
                column: "AppointmentId",
                principalTable: "Appointment",
                principalColumn: "AppointmentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Report_AspNetUsers_DoctorId",
                table: "Report",
                column: "DoctorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Report_Appointment_AppointmentId",
                table: "Report");

            migrationBuilder.DropForeignKey(
                name: "FK_Report_AspNetUsers_DoctorId",
                table: "Report");

            migrationBuilder.DropIndex(
                name: "IX_Report_AppointmentId",
                table: "Report");

            migrationBuilder.DropIndex(
                name: "IX_Report_DoctorId",
                table: "Report");

            migrationBuilder.RenameColumn(
                name: "AppointmentId",
                table: "Report",
                newName: "PatientId");

            migrationBuilder.AlterColumn<string>(
                name: "DoctorId",
                table: "Report",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
