using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthenNet8.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TokenExpires",
                table: "SYS_UserRefreshToken",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "SYS_User",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "SYS_User",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "SYS_User",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "SYS_User",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TokenExpires",
                table: "SYS_UserRefreshToken");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "SYS_User");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "SYS_User");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "SYS_User");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "SYS_User");
        }
    }
}
