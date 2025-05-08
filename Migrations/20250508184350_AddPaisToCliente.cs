using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SistemaContable.Migrations
{
    /// <inheritdoc />
    public partial class AddPaisToCliente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clientes_TiposNcf_TipoNcfId",
                table: "Clientes");

            migrationBuilder.AddColumn<int>(
                name: "PaisId",
                table: "Clientes",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Paises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Codigo = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    Bandera = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paises", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 18, 43, 48, 668, DateTimeKind.Utc).AddTicks(3223));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 18, 43, 48, 668, DateTimeKind.Utc).AddTicks(3226));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 18, 43, 48, 668, DateTimeKind.Utc).AddTicks(3228));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 18, 43, 48, 668, DateTimeKind.Utc).AddTicks(3230));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 18, 43, 48, 668, DateTimeKind.Utc).AddTicks(3231));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 18, 43, 48, 668, DateTimeKind.Utc).AddTicks(3233));

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 8, 18, 43, 48, 668, DateTimeKind.Utc).AddTicks(5057), new DateTime(2025, 5, 8, 18, 43, 48, 668, DateTimeKind.Utc).AddTicks(5058) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 8, 18, 43, 48, 668, DateTimeKind.Utc).AddTicks(5061), new DateTime(2025, 5, 8, 18, 43, 48, 668, DateTimeKind.Utc).AddTicks(5062) });

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 18, 43, 48, 672, DateTimeKind.Utc).AddTicks(6140));

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 18, 43, 48, 672, DateTimeKind.Utc).AddTicks(6152));

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_PaisId",
                table: "Clientes",
                column: "PaisId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clientes_ComprobantesFiscales_TipoNcfId",
                table: "Clientes",
                column: "TipoNcfId",
                principalTable: "ComprobantesFiscales",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Clientes_Paises_PaisId",
                table: "Clientes",
                column: "PaisId",
                principalTable: "Paises",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clientes_ComprobantesFiscales_TipoNcfId",
                table: "Clientes");

            migrationBuilder.DropForeignKey(
                name: "FK_Clientes_Paises_PaisId",
                table: "Clientes");

            migrationBuilder.DropTable(
                name: "Paises");

            migrationBuilder.DropIndex(
                name: "IX_Clientes_PaisId",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "PaisId",
                table: "Clientes");

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 17, 27, 22, 903, DateTimeKind.Utc).AddTicks(9185));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 17, 27, 22, 903, DateTimeKind.Utc).AddTicks(9188));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 17, 27, 22, 903, DateTimeKind.Utc).AddTicks(9190));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 17, 27, 22, 903, DateTimeKind.Utc).AddTicks(9191));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 17, 27, 22, 903, DateTimeKind.Utc).AddTicks(9193));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 17, 27, 22, 903, DateTimeKind.Utc).AddTicks(9195));

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 8, 17, 27, 22, 904, DateTimeKind.Utc).AddTicks(377), new DateTime(2025, 5, 8, 17, 27, 22, 904, DateTimeKind.Utc).AddTicks(378) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 8, 17, 27, 22, 904, DateTimeKind.Utc).AddTicks(380), new DateTime(2025, 5, 8, 17, 27, 22, 904, DateTimeKind.Utc).AddTicks(381) });

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 17, 27, 22, 905, DateTimeKind.Utc).AddTicks(7275));

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 17, 27, 22, 905, DateTimeKind.Utc).AddTicks(7281));

            migrationBuilder.AddForeignKey(
                name: "FK_Clientes_TiposNcf_TipoNcfId",
                table: "Clientes",
                column: "TipoNcfId",
                principalTable: "TiposNcf",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
