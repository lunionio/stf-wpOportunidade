using Microsoft.EntityFrameworkCore.Migrations;

namespace WpOportunidades.Infrastructure.Migrations
{
    public partial class IdClienteParaOportunidadeStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdCliente",
                table: "OportunidadeStatuses",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdCliente",
                table: "OportunidadeStatuses");
        }
    }
}
