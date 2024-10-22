using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoFinalAPI.Migrations
{
    /// <inheritdoc />
    public partial class newwq33 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "usuario",
                table: "OrdenCompra",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "usuario",
                table: "OrdenCompra");
        }
    }
}
