using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class SerialNumberUnique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "status",
                table: "Resources",
                newName: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Scales_SerialNumber",
                table: "Scales",
                column: "SerialNumber",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Scales_SerialNumber",
                table: "Scales");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Resources",
                newName: "status");
        }
    }
}
