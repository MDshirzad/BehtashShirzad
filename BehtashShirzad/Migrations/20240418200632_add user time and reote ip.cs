using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BehtashShirzad.Migrations
{
    /// <inheritdoc />
    public partial class addusertimeandreoteip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "lastIp",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "lastLoginTime",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "lastIp",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "lastLoginTime",
                table: "Users");
        }
    }
}
