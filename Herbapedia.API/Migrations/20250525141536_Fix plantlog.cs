using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Herbapedia.API.Migrations
{
    /// <inheritdoc />
    public partial class Fixplantlog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Plants_PlantLogId",
                table: "Plants");

            migrationBuilder.DropColumn(
                name: "PlantLogId",
                table: "Plants");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaLog",
                table: "Plants",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaLog",
                table: "Plants");

            migrationBuilder.AddColumn<int>(
                name: "PlantLogId",
                table: "Plants",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Plants_PlantLogId",
                table: "Plants",
                column: "PlantLogId");
        }
    }
}
