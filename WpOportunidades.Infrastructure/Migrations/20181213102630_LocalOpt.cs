using Microsoft.EntityFrameworkCore.Migrations;

namespace WpOportunidades.Infrastructure.Migrations
{
    public partial class LocalOpt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LocalOportunidade",
                table: "Enderecos",
                type: "varchar(100)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocalOportunidade",
                table: "Enderecos");
        }
    }
}
