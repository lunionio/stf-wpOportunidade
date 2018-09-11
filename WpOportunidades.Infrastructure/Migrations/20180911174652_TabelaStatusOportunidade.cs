using Microsoft.EntityFrameworkCore.Migrations;

namespace WpOportunidades.Infrastructure.Migrations
{
    public partial class TabelaStatusOportunidade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Status_UserXOportunidades_UserXOportunidadeId",
                table: "Status");

            migrationBuilder.DropIndex(
                name: "IX_Status_UserXOportunidadeId",
                table: "Status");

            migrationBuilder.DropColumn(
                name: "UserXOportunidadeId",
                table: "Status");

            migrationBuilder.AddColumn<int>(
                name: "StatusID",
                table: "UserXOportunidades",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserXOportunidades_StatusID",
                table: "UserXOportunidades",
                column: "StatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_UserXOportunidades_Status_StatusID",
                table: "UserXOportunidades",
                column: "StatusID",
                principalTable: "Status",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserXOportunidades_Status_StatusID",
                table: "UserXOportunidades");

            migrationBuilder.DropIndex(
                name: "IX_UserXOportunidades_StatusID",
                table: "UserXOportunidades");

            migrationBuilder.DropColumn(
                name: "StatusID",
                table: "UserXOportunidades");

            migrationBuilder.AddColumn<int>(
                name: "UserXOportunidadeId",
                table: "Status",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Status_UserXOportunidadeId",
                table: "Status",
                column: "UserXOportunidadeId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Status_UserXOportunidades_UserXOportunidadeId",
                table: "Status",
                column: "UserXOportunidadeId",
                principalTable: "UserXOportunidades",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
