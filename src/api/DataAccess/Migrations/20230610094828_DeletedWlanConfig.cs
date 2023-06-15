using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class DeletedWlanConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WlanName",
                table: "Scales");

            migrationBuilder.DropColumn(
                name: "WlanPassword",
                table: "Scales");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WlanName",
                table: "Scales",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WlanPassword",
                table: "Scales",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
