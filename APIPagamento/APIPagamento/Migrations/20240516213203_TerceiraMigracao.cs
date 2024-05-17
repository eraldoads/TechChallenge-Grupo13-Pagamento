using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable
namespace APIPagamento.Migrations
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    /// <inheritdoc />
    public partial class TerceiraMigracao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pagamento_Pedido_IdPedido",
                table: "Pagamento");

            migrationBuilder.DropTable(
                name: "Pedido");

            migrationBuilder.DropIndex(
                name: "IX_Pagamento_IdPedido",
                table: "Pagamento");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateIndex(
                name: "IX_Pagamento_IdPedido",
                table: "Pagamento",
                column: "IdPedido");

            migrationBuilder.AddForeignKey(
                name: "FK_Pagamento_Pedido_IdPedido",
                table: "Pagamento",
                column: "IdPedido",
                principalTable: "Pedido",
                principalColumn: "IdPedido",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
