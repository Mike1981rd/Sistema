using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SistemaContable.Migrations
{
    /// <inheritdoc />
    public partial class PlazosPagoModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlazosPago",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Dias = table.Column<int>(type: "integer", nullable: true),
                    EsVencimientoManual = table.Column<bool>(type: "boolean", nullable: false),
                    EstaEnUso = table.Column<bool>(type: "boolean", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlazosPago", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "PlazosPago",
                columns: new[] { "Id", "Dias", "EsVencimientoManual", "EstaEnUso", "FechaCreacion", "FechaModificacion", "Nombre" },
                values: new object[,]
                {
                    { 1, 0, false, false, new DateTime(2025, 5, 8, 12, 33, 53, 76, DateTimeKind.Utc).AddTicks(8443), null, "De contado" },
                    { 2, 8, false, false, new DateTime(2025, 5, 8, 12, 33, 53, 76, DateTimeKind.Utc).AddTicks(8446), null, "8 días" },
                    { 3, 15, false, false, new DateTime(2025, 5, 8, 12, 33, 53, 76, DateTimeKind.Utc).AddTicks(8448), null, "15 días" },
                    { 4, 30, false, false, new DateTime(2025, 5, 8, 12, 33, 53, 76, DateTimeKind.Utc).AddTicks(8449), null, "30 días" },
                    { 5, 60, false, false, new DateTime(2025, 5, 8, 12, 33, 53, 76, DateTimeKind.Utc).AddTicks(8451), null, "60 días" },
                    { 6, null, true, false, new DateTime(2025, 5, 8, 12, 33, 53, 76, DateTimeKind.Utc).AddTicks(8453), null, "Vencimiento manual" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlazosPago");
        }
    }
}
