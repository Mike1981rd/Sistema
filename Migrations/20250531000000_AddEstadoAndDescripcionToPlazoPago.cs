using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaContable.Migrations
{
    /// <inheritdoc />
    public partial class AddEstadoAndDescripcionToPlazoPago : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Estado",
                table: "PlazosPago",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "PlazosPago",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            // Actualizar registros existentes para que tengan Estado = true
            migrationBuilder.Sql("UPDATE \"PlazosPago\" SET \"Estado\" = true WHERE \"Estado\" IS NULL OR \"Estado\" = false;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estado",
                table: "PlazosPago");

            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "PlazosPago");
        }
    }
}