using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthenNet8.Migrations
{
    /// <inheritdoc />
    public partial class Update1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "SYS_UserRefreshToken",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "SYS_UserRefreshToken",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "SYS_UserRefreshToken",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "SYS_UserRefreshToken",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "SYS_UserRefreshToken");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "SYS_UserRefreshToken");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "SYS_UserRefreshToken");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "SYS_UserRefreshToken");
        }
    }
}
