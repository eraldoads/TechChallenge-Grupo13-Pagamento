using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIPagamento.Migrations
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    /// <inheritdoc />
    public partial class PrimeiraMigracao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Pedido",
                columns: table => new
                {
                    IdPedido = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedido", x => x.IdPedido);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Pagamento",
                columns: table => new
                {
                    IdPagamento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StatusPagamento = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ValorPagamento = table.Column<float>(type: "float", nullable: false),
                    MetodoPagamento = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataPagamento = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IdPedido = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagamento", x => x.IdPagamento);
                    table.ForeignKey(
                        name: "FK_Pagamento_Pedido_IdPedido",
                        column: x => x.IdPedido,
                        principalTable: "Pedido",
                        principalColumn: "IdPedido",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Pagamento_IdPedido",
                table: "Pagamento",
                column: "IdPedido");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pagamento");

            migrationBuilder.DropTable(
                name: "Pedido");
        }
    }
}
