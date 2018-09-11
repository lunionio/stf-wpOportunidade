using Microsoft.EntityFrameworkCore.Migrations;

namespace WpOportunidades.Infrastructure.Migrations
{
    public partial class TabelaEnderecos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Endereco_Oportunidades_OportunidadeId",
                table: "Endereco");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Endereco",
                table: "Endereco");

            migrationBuilder.RenameTable(
                name: "Endereco",
                newName: "Enderecos");

            migrationBuilder.RenameIndex(
                name: "IX_Endereco_OportunidadeId",
                table: "Enderecos",
                newName: "IX_Enderecos_OportunidadeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Enderecos",
                table: "Enderecos",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Enderecos_Oportunidades_OportunidadeId",
                table: "Enderecos",
                column: "OportunidadeId",
                principalTable: "Oportunidades",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enderecos_Oportunidades_OportunidadeId",
                table: "Enderecos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Enderecos",
                table: "Enderecos");

            migrationBuilder.RenameTable(
                name: "Enderecos",
                newName: "Endereco");

            migrationBuilder.RenameIndex(
                name: "IX_Enderecos_OportunidadeId",
                table: "Endereco",
                newName: "IX_Endereco_OportunidadeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Endereco",
                table: "Endereco",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Endereco_Oportunidades_OportunidadeId",
                table: "Endereco",
                column: "OportunidadeId",
                principalTable: "Oportunidades",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
