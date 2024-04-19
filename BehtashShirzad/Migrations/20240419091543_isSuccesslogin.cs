using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BehtashShirzad.Migrations
{
    /// <inheritdoc />
    public partial class isSuccesslogin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isLastLoginSuccessfull",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isLastLoginSuccessfull",
                table: "Users");
        }
    }
}
