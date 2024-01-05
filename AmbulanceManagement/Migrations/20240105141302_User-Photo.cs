using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmbulanceManagement.Migrations
{
    public partial class UserPhoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfilePictureContentType",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte[]>(
                name: "ProfilePictureData",
                table: "AspNetUsers",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePictureContentType",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProfilePictureData",
                table: "AspNetUsers");
        }
    }
}
