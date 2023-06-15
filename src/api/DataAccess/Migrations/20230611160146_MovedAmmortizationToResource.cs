using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class MovedAmmortizationToResource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Measurements_MoneyAmount_AmmortizationId",
                table: "Measurements");

            migrationBuilder.DropIndex(
                name: "IX_Measurements_AmmortizationId",
                table: "Measurements");

            migrationBuilder.DropColumn(
                name: "AmmortizationId",
                table: "Measurements");

            migrationBuilder.AddColumn<int>(
                name: "AmmortizationId",
                table: "Resources",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Resources_AmmortizationId",
                table: "Resources",
                column: "AmmortizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Resources_MoneyAmount_AmmortizationId",
                table: "Resources",
                column: "AmmortizationId",
                principalTable: "MoneyAmount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resources_MoneyAmount_AmmortizationId",
                table: "Resources");

            migrationBuilder.DropIndex(
                name: "IX_Resources_AmmortizationId",
                table: "Resources");

            migrationBuilder.DropColumn(
                name: "AmmortizationId",
                table: "Resources");

            migrationBuilder.AddColumn<int>(
                name: "AmmortizationId",
                table: "Measurements",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Measurements_AmmortizationId",
                table: "Measurements",
                column: "AmmortizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Measurements_MoneyAmount_AmmortizationId",
                table: "Measurements",
                column: "AmmortizationId",
                principalTable: "MoneyAmount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
