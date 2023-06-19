using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class FixedManyToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resources_WorkingShifts_WorkingShiftId",
                table: "Resources");

            migrationBuilder.DropIndex(
                name: "IX_Resources_WorkingShiftId",
                table: "Resources");

            migrationBuilder.DropColumn(
                name: "WorkingShiftId",
                table: "Resources");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Start",
                table: "WorkingShifts",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "End",
                table: "WorkingShifts",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateTime",
                table: "Measurements",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.CreateTable(
                name: "ResourceWorkingShift",
                columns: table => new
                {
                    UsedResourcesId = table.Column<int>(type: "integer", nullable: false),
                    usedInId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceWorkingShift", x => new { x.UsedResourcesId, x.usedInId });
                    table.ForeignKey(
                        name: "FK_ResourceWorkingShift_Resources_UsedResourcesId",
                        column: x => x.UsedResourcesId,
                        principalTable: "Resources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResourceWorkingShift_WorkingShifts_usedInId",
                        column: x => x.usedInId,
                        principalTable: "WorkingShifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResourceWorkingShift_usedInId",
                table: "ResourceWorkingShift",
                column: "usedInId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResourceWorkingShift");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Start",
                table: "WorkingShifts",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "End",
                table: "WorkingShifts",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkingShiftId",
                table: "Resources",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateTime",
                table: "Measurements",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.CreateIndex(
                name: "IX_Resources_WorkingShiftId",
                table: "Resources",
                column: "WorkingShiftId");

            migrationBuilder.AddForeignKey(
                name: "FK_Resources_WorkingShifts_WorkingShiftId",
                table: "Resources",
                column: "WorkingShiftId",
                principalTable: "WorkingShifts",
                principalColumn: "Id");
        }
    }
}
