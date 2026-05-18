using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Herbapedia.API.Migrations
{
    /// <inheritdoc />
    public partial class ActualizacionesVarias2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Discriminator",
                table: "Plants",
                newName: "Log");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Log",
                table: "Plants",
                newName: "Discriminator");
        }
    }
}
