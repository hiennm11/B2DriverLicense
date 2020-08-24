using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace B2DriverLicense.Core.Migrations
{
    public partial class ImageAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "Questions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageTitle",
                table: "Questions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "ImageTitle",
                table: "Questions");
        }
    }
}
