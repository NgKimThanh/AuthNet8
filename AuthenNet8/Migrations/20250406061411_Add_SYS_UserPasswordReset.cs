using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AuthenNet8.Migrations
{
    /// <inheritdoc />
    public partial class Add_SYS_UserPasswordReset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "Password",
            //    table: "SYS_User");

            //migrationBuilder.DropColumn(
            //    name: "RefreshToken",
            //    table: "SYS_User");

            //migrationBuilder.DropColumn(
            //    name: "TokenCreated",
            //    table: "SYS_User");

            //migrationBuilder.DropColumn(
            //    name: "TokenExpires",
            //    table: "SYS_User");

            migrationBuilder.CreateTable(
                name: "SYS_UserPasswordReset",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserID = table.Column<int>(type: "integer", nullable: false),
                    ResetToken = table.Column<string>(type: "text", nullable: false),
                    TokenExpires = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_UserPasswordReset", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SYS_UserPasswordReset_SYS_User_UserID",
                        column: x => x.UserID,
                        principalTable: "SYS_User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SYS_UserPasswordReset_UserID",
                table: "SYS_UserPasswordReset",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SYS_UserPasswordReset");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "SYS_User",
                type: "character varying(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "SYS_User",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TokenCreated",
                table: "SYS_User",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TokenExpires",
                table: "SYS_User",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}
