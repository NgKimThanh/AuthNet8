using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AuthenNet8.Migrations
{
    /// <inheritdoc />
    public partial class SYS_User : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SYS_User",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    PasswordSalt = table.Column<string>(type: "text", nullable: true),
                    RefreshToken = table.Column<string>(type: "text", nullable: true),
                    TokenCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TokenExpires = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_User", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_UserRefreshToken",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserID = table.Column<int>(type: "integer", nullable: true),
                    Token = table.Column<string>(type: "text", nullable: false),
                    DeviceInfo = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_UserRefreshToken", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SYS_UserRefreshToken_SYS_User_UserID",
                        column: x => x.UserID,
                        principalTable: "SYS_User",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SYS_UserRefreshToken_UserID",
                table: "SYS_UserRefreshToken",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SYS_UserRefreshToken");

            migrationBuilder.DropTable(
                name: "SYS_User");
        }
    }
}
