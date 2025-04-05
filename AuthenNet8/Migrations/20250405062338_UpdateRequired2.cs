using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthenNet8.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRequired2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SYS_UserRefreshToken_SYS_User_UserID",
                table: "SYS_UserRefreshToken");

            migrationBuilder.AlterColumn<int>(
                name: "UserID",
                table: "SYS_UserRefreshToken",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SYS_UserRefreshToken_SYS_User_UserID",
                table: "SYS_UserRefreshToken",
                column: "UserID",
                principalTable: "SYS_User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SYS_UserRefreshToken_SYS_User_UserID",
                table: "SYS_UserRefreshToken");

            migrationBuilder.AlterColumn<int>(
                name: "UserID",
                table: "SYS_UserRefreshToken",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_SYS_UserRefreshToken_SYS_User_UserID",
                table: "SYS_UserRefreshToken",
                column: "UserID",
                principalTable: "SYS_User",
                principalColumn: "ID");
        }
    }
}
